using ProductCatalog.Application;
using ProductCatalog.Application.ValueObjects;
using Xunit;

namespace ProductCatalog.Tests.UnitTests;

public class ProductTests
{
    [Theory] 
    [InlineData(null)] 
    [InlineData("")]
    [InlineData("too long name of the product")]
    public void cannot_create_product_when_name_is_invalid(string name) =>
        Assert.Throws<InvalidNameException>(
            () => new Product(Guid.NewGuid().ToString(), name, string.Empty, 0));

    [Fact]
    public void cannot_create_product_when_description_is_invalid() =>
        Assert.Throws<InvalidDescriptionException>(
            () => new Product(
                Guid.NewGuid().ToString(), "test", 
                "this is too long description that should not allow to create product", 
                0));
    
}