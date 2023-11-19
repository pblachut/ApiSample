using ProductCatalog.Application.ProductRegistration;
using ProductCatalog.Contract;

namespace ProductCatalog.Application.DatabaseInitialization;

public class InitializeDatabase : IHostedService
{
    private readonly RegisterProduct _registerProduct;

    public InitializeDatabase(RegisterProduct registerProduct)
    {
        _registerProduct = registerProduct;
    }

    private static readonly Commands.RegisterProduct[] CommandsToFire = new[]
    {
        new Commands.RegisterProduct("Fruits", "Delicious fruits", 2.34m),
        new Commands.RegisterProduct("Book about pets", "Fantastic book about pets", 3.78m),
        new Commands.RegisterProduct("Book about hobbies", "Buy it to find out more about hobbies", 4.51m),
        new Commands.RegisterProduct("Travels", "Travels around the world",2.09m),
        new Commands.RegisterProduct("Food preparation", "Book about food preparation", 1.12m)
    };
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Task.WhenAll(
            CommandsToFire
                .Select(command => _registerProduct(command, cancellationToken))
        );
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}