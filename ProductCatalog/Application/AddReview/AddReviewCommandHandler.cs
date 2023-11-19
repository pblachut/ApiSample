using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Contract;
using static ProductCatalog.Application.AddReview.ExpectedExceptions;

namespace ProductCatalog.Application.AddReview;

public class AddReviewCommandHandler: ControllerBase
{
    private readonly AddReview _addReview;

    public AddReviewCommandHandler(AddReview addReview)
    {
        _addReview = addReview;
    }

    [HttpPost("addReview")]
    public async Task<IActionResult> AddReview(Commands.AddReview command, CancellationToken token)
    {
        var (id, error) = await _addReview(command, token);

        return (id, error) switch
        {
            (_, null) => Ok(new Commands.AddReview.Response(id)),
            (_, not null) when IsExpectedException(error) => Problem(error.Message, statusCode: StatusCodes.Status400BadRequest),
            _ => Problem(error.Message, statusCode: StatusCodes.Status500InternalServerError)
        };
    }
}