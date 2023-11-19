using Microsoft.EntityFrameworkCore;
using ProductCatalog.Infrastructure;

namespace ProductCatalog.Application.GetProduct;

public delegate Task<(Contract.Product? Result, bool WasFound)> GetProduct(string id, CancellationToken token);

public static class GetProductSetup
{
    public static GetProduct CreateGetProduct(Func<DatabaseContext> getDatabaseContext) =>
        async (id, token) =>
        {
            await using var dbContext = getDatabaseContext();

            var product = await dbContext.Products.SingleOrDefaultAsync(s => s.Id == id, token);

            return (product, WasFound());

            bool WasFound() => product != null;
        };

    public static GetProduct WithLogging(this GetProduct getProduct, ILogger logger) =>
        async (id, token) =>
        {
            try
            {
                return await getProduct(id, token);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error during getting product with id {Id}", id);
                throw;
            }
        }; 
}