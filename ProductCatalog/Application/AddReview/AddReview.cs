using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.ValueObjects;
using ProductCatalog.Contract;
using ProductCatalog.Infrastructure;
using static ProductCatalog.Application.AddReview.ExpectedExceptions;

namespace ProductCatalog.Application.AddReview;

public delegate Task<(string Id, Exception? Error)> AddReview(Commands.AddReview command, CancellationToken token);

public static class AddReviewSetup
{
    public static AddReview CreateAddReview(Func<DatabaseContext> getDatabaseContext) =>
        async (command, token) =>
        {
            var id = Guid.NewGuid().ToString();

            var rate = Rate.From(command.Rate);

            var review = new Review(id, command.ProductId, command.ReviewerId, rate.Value, command.Details, DateTimeOffset.UtcNow);

            await using var dbContext = getDatabaseContext();

            await dbContext.Reviews.AddAsync(review, token);

            await dbContext.SaveChangesAsync(token);

            return (id, default);
        };

    public static AddReview ThrowWhenProductDoesntExist(this AddReview addReview, GetProduct.GetProduct getProduct) =>
        async (command, token) =>
        {
            var (_, wasFound) = await getProduct(command.ProductId, token);
            if (wasFound == false) return (string.Empty, new NotPossibleToAddReviewException("Product was not found"));

            return await addReview(command, token);
        };
    
    public static AddReview ThrowWhenReviewerAlreadyCreatedReviewForProduct(this AddReview addReview, Func<DatabaseContext> getDatabaseContext) =>
        async (command, token) =>
        {
            var review = await TryGetExistingReview();
            if (review is not null) return (string.Empty, new NotPossibleToAddReviewException("Reviewer already created review for product"));

            return await addReview(command, token);
            

            Task<Review?> TryGetExistingReview()
            {
                var databaseContext = getDatabaseContext();

                return databaseContext.Reviews
                    .SingleOrDefaultAsync(
                        c => c.ProductId == command.ProductId && c.ReviewerId == command.ReviewerId, 
                        cancellationToken: token
                    );
            }
        };
    
    public static AddReview WithLogging(this AddReview addReview, ILogger logger) =>
        async (command, token) =>
        {
            try
            {
                return await addReview(command, token);
            }
            catch (Exception e) when (IsExpectedException(e) == false)
            {
                logger.LogError(e, "Error during adding review {Command}", command);
                throw;
            }
        };
    
    public static AddReview WithExceptionHandling(this AddReview addReview) =>
        async (command, token) =>
        {
            try
            {
                return await addReview(command, token);
            }
            catch (Exception e)
            {
                return (string.Empty, e);
            }
        };

}

public class NotPossibleToAddReviewException : Exception
{
    public NotPossibleToAddReviewException(string details) 
        :base(details){}
}