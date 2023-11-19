using ProductCatalog.Application.AddReview;
using ProductCatalog.Application.ApplyProductReviews;
using ProductCatalog.Application.DatabaseInitialization;
using ProductCatalog.Application.GetProduct;
using ProductCatalog.Application.ProductRegistration;
using ProductCatalog.Application.ProductSearch;
using ProductCatalog.Infrastructure;

namespace ProductCatalog.Application
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection WithApplication(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton(sp =>
                    SearchProductsSetup
                        .CreateSearchProducts(sp.GetDataContext)
                        .WithLogging(sp.GetRequiredService<ILogger<SearchProducts>>())
                )
                .AddSingleton(sp =>
                    RegisterProductSetup
                        .CreateRegisterProduct(sp.GetDataContext)
                        .WithLogging(sp.GetRequiredService<ILogger<RegisterProduct>>())
                        .WithExceptionHandling()
                )
                .AddSingleton(sp =>
                    GetProductSetup
                        .CreateGetProduct(sp.GetDataContext)
                        .WithLogging(sp.GetRequiredService<ILogger<GetProduct.GetProduct>>())
                )
                .AddSingleton(sp =>
                    AddReviewSetup
                        .CreateAddReview(sp.GetDataContext)
                        .ThrowWhenProductDoesntExist(sp.GetRequiredService<GetProduct.GetProduct>())
                        .ThrowWhenReviewerAlreadyCreatedReviewForProduct(sp.GetDataContext)
                        .WithLogging(sp.GetRequiredService<ILogger<AddReview.AddReview>>())
                        .WithExceptionHandling()
                )
                .AddHostedService<InitializeDatabase>()
                .WithApplyProductRating();
    }
}