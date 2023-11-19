using Microsoft.EntityFrameworkCore;
using ProductCatalog.Contract;
using ProductCatalog.Infrastructure;

namespace ProductCatalog.Tests.IntegrationTests;

public class DatabaseFixture
{
    private Product[] _toStore = Array.Empty<Product>();

    public DatabaseFixture WithProducts(params Product[] toStore)
    {
        _toStore = toStore;

        return this;
    }

    public async Task ClearAndStore()
    {
        await ClearDatabase();
        
        if (!_toStore.Any()) return;

        await StoreSurveys();
    }

    public async Task ClearDatabase()
    {
        await using var dbContext = new DatabaseContext();

        var toRemove = await dbContext.Products.ToArrayAsync();
        
        dbContext.Products.RemoveRange(toRemove);

        await dbContext.SaveChangesAsync();
    }

    private async Task StoreSurveys()
    {
        await using var dbContext = new DatabaseContext();

        await dbContext.Products.AddRangeAsync(_toStore);

        await dbContext.SaveChangesAsync();
    }
}