using ProductCatalog.Contract;

namespace ProductCatalog.Tests.IntegrationTests;

public class ProductBuilder
{
    private string _id = Guid.NewGuid().ToString();
    private string _name = "test name";
    private string _description = "sample description";
    private decimal _price = 2.3m;
    private double _rate = 5.5;
    private int _numberOfReviews = 0;

    public ProductBuilder WithName(string name)
    {
        _name = name;

        return this;
    }

    public ProductBuilder WithRate(double rate)
    {
        _rate = rate;

        return this;
    }

    public Product Build() =>
        new(_id,
            _name,
            _description,
            _price,
            _rate,
            _numberOfReviews
        );
}