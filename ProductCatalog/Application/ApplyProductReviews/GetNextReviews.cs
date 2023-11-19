using Microsoft.EntityFrameworkCore;
using ProductCatalog.Contract;
using ProductCatalog.Infrastructure;

namespace ProductCatalog.Application.ApplyProductReviews;

public delegate Task<Review[]> GetNextReviews(DateTimeOffset createdAfter, CancellationToken token);

public static class GetNextReviewsSetup
{
    public static GetNextReviews CreateGetNextReviews(Func<DatabaseContext> getDatabaseContext) =>
        async (createdAfter, token) =>
        {
            var context = getDatabaseContext();

            return await context.Reviews.Where(c => c.CreatedAt > createdAfter)
                .Take(10)
                .ToArrayAsync(cancellationToken: token);
        };
}