using ProductCatalog.Application.ValueObjects;

namespace ProductCatalog.Application;

public static class Mappings
{
    public static Contract.Product ToContract(this Product product) =>
        new(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Rate?.Value,
            product.NumberOfReviews
        );

    public static Product ToModel(this Contract.Product product) =>
        new(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Rate != null ? Rate.From(product.Rate.Value) : null,
            product.NumberOfReviews
        );
}