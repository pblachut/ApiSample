using ProductCatalog.Contract;

namespace ProductCatalog.Application.ApplyProductReviews;

public delegate Task ApplyProductReviews(CancellationToken token);

public static class ApplyProductReviewsSetup
{
    public static ApplyProductReviews CreateApplyProductRating(
        GetCheckpoint getCheckpoint, StoreCheckpoint storeCheckpoint, GetNextReviews getNextReviews,
        RefreshProductsRate refreshProductsRate) =>
        async token =>
        {
            var checkpoint = await getCheckpoint(token);

            while (!token.IsCancellationRequested)
            {
                var reviews = await getNextReviews(checkpoint, token);
                if (reviews.Any())
                {
                    await ApplyRating(reviews);

                    checkpoint = reviews.OrderByDescending(c => c.CreatedAt).Select(c => c.CreatedAt).First();

                    await storeCheckpoint(checkpoint, token);
                }
                else
                {
                    await Task.Delay(TimeSpan.FromMinutes(1), token);
                }
            }
            
            Task ApplyRating(IEnumerable<Review> reviews)
            {
                var commands = reviews.GroupBy(c => c.ProductId)
                    .Select(c => new Commands.RefreshProductRate(c.Key))
                    .ToArray();

                return refreshProductsRate(commands, token);
            }
        };

    public static ApplyProductReviews WithLogging(this ApplyProductReviews applyProductReviews, ILogger logger) =>
        async token =>
        {
            try
            {
                await applyProductReviews(token);
            }
            catch (TaskCanceledException)
            { }
            catch (Exception e)
            {
                logger.LogError(e, "Error during applying product reviews");
                throw;  
            }
        };
}