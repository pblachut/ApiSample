using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Contract;
using ProductCatalog.Infrastructure;

namespace ProductCatalog.Tests.IntegrationTests;

public class ApiFixture
{
    private readonly HttpClient _client;
    
    public ApiFixture(WebApplicationFactory<Program> factory) =>
        _client = factory.CreateClient();
    
    public Task<(Commands.RegisterProduct.Response? Response, HttpStatusCode StatusCode)> RegisterProduct(Commands.RegisterProduct command)
        => ReturnResult<Commands.RegisterProduct.Response>(() => _client.PostAsJsonAsync("/registerProduct", command));

    public Task<(Queries.SearchProducts.Response? Response, HttpStatusCode StatusCode)> SearchProducts(
        string nameContains)
        => ReturnResult<Queries.SearchProducts.Response>(() => _client.GetAsync($"/products/search?name={nameContains}"));
    
    public Task<(Queries.GetProduct.Response? Response, HttpStatusCode StatusCode)> GetProduct(
        string id)
        => ReturnResult<Queries.GetProduct.Response>(() => _client.GetAsync($"/products/{id}"));
    
    async Task<(TResult? Response, HttpStatusCode StatusCode)> ReturnResult<TResult>(
        Func<Task<HttpResponseMessage>> getResponse)
    {
        var response = await getResponse();

        if (!response.IsSuccessStatusCode) return (default, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<TResult>();

        return (result, response.StatusCode);
    }

    public async Task<Product[]> GetAllProductsFromDatabase()
    {
        await using var dbContext = new DatabaseContext();

        return await dbContext.Products.ToArrayAsync();
    }
}