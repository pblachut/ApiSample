using Microsoft.EntityFrameworkCore;
using DatabaseContext = ProductCatalog.Infrastructure.DatabaseContext;

namespace ProductCatalog.Application.ProductSearch;

public delegate Task<Contract.Product[]> SearchProducts(string? nameContains, int skip, int take, CancellationToken token);

public static class SearchProductsSetup
{
    public static SearchProducts CreateSearchProducts(Func<DatabaseContext> getDatabaseContext) =>
        async (nameContains, skip, take, token) =>
        {
            await using var dbContext = getDatabaseContext();
            
            return await GetQuery()
                .OrderByDescending(c => c.Rate)
                .Skip(skip)
                .Take(take)
                .ToArrayAsync(cancellationToken: token);

            IQueryable<Contract.Product> GetQuery() =>
                string.IsNullOrEmpty(nameContains)
                    ? dbContext.Products.AsQueryable()
                    : dbContext.Products
                        .Where(s => s.Name.Contains(nameContains, StringComparison.InvariantCultureIgnoreCase));
        };

    public static SearchProducts WithLogging(this SearchProducts searchProducts, ILogger logger) =>
        async (nameContains, skip, take, token) =>
        {
            try
            {
                return await searchProducts(nameContains, skip, take, token);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error during getting products");
                throw;
            }
        };  

}