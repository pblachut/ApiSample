namespace ProductCatalog.Contract;

public record Review(
    string Id, 
    string ProductId, 
    string ReviewerId, 
    double Rate, 
    string Details, 
    DateTimeOffset CreatedAt
);