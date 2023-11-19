using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Contract;
using static ProductCatalog.Application.ProductSearch.SearchProductsResponseFactory;

namespace ProductCatalog.Application.ProductSearch;

public class SearchProductsQuery : ControllerBase
{
    private readonly SearchProducts _searchProducts;

    public SearchProductsQuery(SearchProducts searchProducts)
    {
        _searchProducts = searchProducts;
    }

    [HttpGet("products/search")]
    public async Task<IActionResult> Search([FromQuery] Queries.SearchProducts query, CancellationToken token)
    {
        var products = await _searchProducts(query.Name, query.Skip, query.Take, token);

        return Ok(CreateResponse(products));
    }
}