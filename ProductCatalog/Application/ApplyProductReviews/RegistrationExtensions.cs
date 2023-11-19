using ProductCatalog.Infrastructure;

namespace ProductCatalog.Application.ApplyProductReviews;

public static class RegistrationExtensions
{
    public static IServiceCollection WithApplyProductRating(this IServiceCollection serviceCollection) =>
        serviceCollection
            .AddHostedService<ApplyProductRatingBootstrap>()
            .AddSingleton(sp =>
                CheckpointPersistence
                    .CreateGetCheckpoint(sp.GetDataContext)
            )
            .AddSingleton(sp =>
                CheckpointPersistence
                    .CreateStoreCheckpoint(sp.GetDataContext)
            )
            .AddSingleton(sp =>
                GetNextReviewsSetup
                    .CreateGetNextReviews(sp.GetDataContext)
            )
            .AddSingleton(sp =>
                RefreshProductsRateSetup
                    .CreateRefreshProductsRate(sp.GetDataContext)
            )
            .AddSingleton(sp =>
                ApplyProductReviewsSetup
                    .CreateApplyProductRating(
                        sp.GetRequiredService<GetCheckpoint>(),
                        sp.GetRequiredService<StoreCheckpoint>(),
                        sp.GetRequiredService<GetNextReviews>(),
                        sp.GetRequiredService<RefreshProductsRate>()
                    )
                    .WithLogging(sp.GetRequiredService<ILogger<ApplyProductReviews>>())
            )
        ;
}