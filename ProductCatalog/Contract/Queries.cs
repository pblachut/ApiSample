namespace ProductCatalog.Contract;

public static class Queries
{
    public record SearchProducts(string? Name, int Skip = 0, int Take = 20)
    {
        public record Response(SearchResult[] Results);

        public record SearchResult(
            string Id,
            string Name,
            double? Rate
        );
    }

    public record GetProduct(string Id)
    {
        public record Response(
            string Id,
            string Name,
            string Description,
            decimal Price
        );
    }
}