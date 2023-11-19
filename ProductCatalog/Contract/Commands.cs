namespace ProductCatalog.Contract;

public static class Commands
{
    public record RegisterProduct(
        string Name,
        string Description,
        decimal Price
    )
    {
        public record Response(string Id);
    }

    public record AddReview(string ProductId, string ReviewerId, double Rate, string Details)
    {
        public record Response(string Id); 
    }

    public record RefreshProductRate(string ProductId);
}