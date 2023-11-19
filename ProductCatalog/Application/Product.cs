using ProductCatalog.Application.ValueObjects;

namespace ProductCatalog.Application;

public record Product(
    string Id,
    Name Name,
    Description Description,
    AmountInEuros Price,
    Rate? Rate = null,
    int NumberOfReviews = 0)
{
    public Product ChangeRate(Rate rate, int numberOfReviews) =>
        this with
        {
            Rate = rate,
            NumberOfReviews = numberOfReviews
        };
}