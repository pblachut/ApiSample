namespace ProductCatalog.Application.ApplyProductReviews;

public class ApplyProductRatingBootstrap : IHostedService
{
    private readonly ApplyProductReviews _applyProductReviews;
    private readonly CancellationTokenSource _tokenSource = new();
    private Task? _poolingTask;
    
    public ApplyProductRatingBootstrap(ApplyProductReviews applyProductReviews)
    {
        _applyProductReviews = applyProductReviews;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _poolingTask = _applyProductReviews(_tokenSource.Token);

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _tokenSource.Cancel();

        if (_poolingTask is not null)
            await _poolingTask;
    }
}