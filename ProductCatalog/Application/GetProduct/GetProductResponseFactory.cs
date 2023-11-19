using ProductCatalog.Contract;

namespace ProductCatalog.Application.GetProduct;

public static class GetProductResponseFactory
{
    public static Queries.GetProduct.Response CreateResponse(Contract.Product product) =>
        new(
            product.Id,
            product.Name,
            product.Description,
            product.Price
        );
}