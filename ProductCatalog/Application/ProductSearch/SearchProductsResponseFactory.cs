using ProductCatalog.Contract;

namespace ProductCatalog.Application.ProductSearch;

public static class SearchProductsResponseFactory
{
    public static Queries.SearchProducts.Response CreateResponse(IEnumerable<Contract.Product> products) =>
        new(products
            .Select(MapToResult)
            .ToArray()
        );

    static Queries.SearchProducts.SearchResult MapToResult(Contract.Product product) =>
        new(
            product.Id,
            product.Name,
            product.Rate
        );
}