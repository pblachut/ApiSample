using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Contract;
using static ProductCatalog.Application.GetProduct.GetProductResponseFactory;

namespace ProductCatalog.Application.GetProduct;

public class GetProductQuery : ControllerBase
{
    private readonly GetProduct _getProduct;

    public GetProductQuery(GetProduct getProduct)
    {
        _getProduct = getProduct;
    }

    [HttpGet("products/{id}")]
    public async Task<IActionResult> GetProduct([FromRoute] Queries.GetProduct query, CancellationToken token)
    {
        var (product, wasFound) = await _getProduct(query.Id, token);

        return (product, wasFound) switch
        {
            (not null, true) => Ok(CreateResponse(product)),
            (_, false) => Problem(statusCode: StatusCodes.Status404NotFound),
            _ => Problem(statusCode: StatusCodes.Status500InternalServerError)
        };
    }
}