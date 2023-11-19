using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Contract;
using static ProductCatalog.Application.ProductRegistration.ExpectedExceptions;

namespace ProductCatalog.Application.ProductRegistration;

public class RegisterProductCommandHandler : ControllerBase
{
    private readonly RegisterProduct _registerProduct;

    public RegisterProductCommandHandler(RegisterProduct registerProduct)
    {
        _registerProduct = registerProduct;
    }

    [HttpPost("registerProduct")]
    public async Task<IActionResult> RegisterProducts([FromBody] Commands.RegisterProduct command, CancellationToken token)
    {
        var (id, error) = await _registerProduct(command, token);

        return (id, error) switch
        {
            (_, null) => Ok(new Commands.RegisterProduct.Response(id)),
            (_, not null) when IsExpectedException(error) => Problem(error.Message, statusCode: StatusCodes.Status400BadRequest),
            _ => Problem(error.Message, statusCode: StatusCodes.Status500InternalServerError)
        };
    }
}