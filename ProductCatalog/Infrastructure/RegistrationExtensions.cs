namespace ProductCatalog.Infrastructure;

public static class RegistrationExtensions
{
    public static IServiceCollection WithInfrastructure(this IServiceCollection serviceCollection) =>
        serviceCollection.AddDbContext<DatabaseContext>();



    public static DatabaseContext GetDataContext(this IServiceProvider sp) =>
        sp.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();
}