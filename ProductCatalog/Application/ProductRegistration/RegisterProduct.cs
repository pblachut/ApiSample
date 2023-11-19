using ProductCatalog.Contract;
using ProductCatalog.Infrastructure;

namespace ProductCatalog.Application.ProductRegistration;

public delegate Task<(string Id, Exception? Error)> RegisterProduct(Commands.RegisterProduct command, CancellationToken token);

public static class RegisterProductSetup
{
    public static RegisterProduct CreateRegisterProduct(Func<DatabaseContext> getDatabaseContext) =>
        async (command, token) =>
        {
            var id = Guid.NewGuid().ToString();

            var product = new Product(id,
                command.Name, command.Description,
                command.Price
            );

            await using var dbContext = getDatabaseContext();

            var contract = product.ToContract();

            await dbContext.Products.AddAsync(contract, token);

            await dbContext.SaveChangesAsync(token);

            return (id, null);
        };

    public static RegisterProduct WithLogging(this RegisterProduct registerProduct, ILogger logger) =>
        async (command, token) =>
        {
            try
            {
                return await registerProduct(command, token);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error during saving product: {Command}", command);
                throw;
            }
        };
    
    public static RegisterProduct WithExceptionHandling(this RegisterProduct registerProduct) =>
        async (command, token) =>
        {
            try
            {
                return await registerProduct(command, token);
            }
            catch (Exception e)
            {
                return (default, e);
            }
        };
}