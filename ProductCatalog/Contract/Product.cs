namespace ProductCatalog.Contract;

public record Product(
    string Id,
    string Name,
    string Description,
    decimal Price,
    double? Rate,
    int NumberOfReviews
);