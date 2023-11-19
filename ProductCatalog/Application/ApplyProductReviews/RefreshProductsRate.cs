using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.ValueObjects;
using ProductCatalog.Contract;
using ProductCatalog.Infrastructure;

namespace ProductCatalog.Application.ApplyProductReviews;

public delegate Task RefreshProductsRate(Commands.RefreshProductRate[] commands, CancellationToken token);

public static class RefreshProductsRateSetup
{
    public static RefreshProductsRate CreateRefreshProductsRate(Func<DatabaseContext> getDatabaseContext) =>
        async (commands, token) =>
        {
            var context = getDatabaseContext();

            var products = await context.Products
                .AsNoTracking()
                .Where(c => commands.Select(d => d.ProductId).Contains(c.Id))
                .ToArrayAsync(token);

            foreach (var command in commands)
            {
                var oldProduct = products.Single(c => c.Id == command.ProductId);
                
                var newProduct = await RefreshProductRate(oldProduct.ToModel(), command);

                context.Products.Update(newProduct);
            }

            await context.SaveChangesAsync(token);
            
            async Task<Contract.Product> RefreshProductRate(Product product, Commands.RefreshProductRate command)
            {
                var (rate, numberOfReviews) = await SummarizeExistingReviews();

                return product
                    .ChangeRate(Rate.From(rate), numberOfReviews)
                    .ToContract();

                async Task<(double Rate, int NumberOfReviews)> SummarizeExistingReviews()
                {
                    return (await GetRate(), await GetNumberOfReviews());

                    Task<double> GetRate() =>
                        context.Reviews
                            .Where(r => r.ProductId == product.Id)
                            .Select(c => c.Rate)
                            .AverageAsync(token);

                    Task<int> GetNumberOfReviews() =>
                        context.Reviews
                            .Where(r => r.ProductId == product.Id)
                            .CountAsync(token);
                }
            }
        };
}