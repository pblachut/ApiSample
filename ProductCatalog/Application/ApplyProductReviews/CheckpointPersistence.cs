using Microsoft.EntityFrameworkCore;
using ProductCatalog.Infrastructure;

namespace ProductCatalog.Application.ApplyProductReviews;

// ReSharper disable once NotAccessedPositionalProperty.Global
public record Checkpoint(DateTimeOffset Timestamp, string Id = "Default");

public delegate Task<DateTimeOffset> GetCheckpoint(CancellationToken token);

public delegate Task StoreCheckpoint(DateTimeOffset timestamp, CancellationToken token);

public static class CheckpointPersistence
{
    public static GetCheckpoint CreateGetCheckpoint(Func<DatabaseContext> getDatabaseContext) =>
        async token =>
        {
            var context = getDatabaseContext();

            var checkpoint = await context.Checkpoints.SingleOrDefaultAsync(token);

            return checkpoint?.Timestamp ?? DateTimeOffset.MinValue;
        };

    public static StoreCheckpoint CreateStoreCheckpoint(Func<DatabaseContext> getDatabaseContext) =>
        async (timestamp, token) =>
        {
            var context = getDatabaseContext();

            var existingCheckpoint = await context.Checkpoints
                .AsNoTracking()
                .SingleOrDefaultAsync(token);

            if (existingCheckpoint is null)
            {
                context.Checkpoints.Add(new Checkpoint(timestamp));
            }
            else
            {
                existingCheckpoint = new Checkpoint(timestamp);

                context.Checkpoints.Update(existingCheckpoint);
            }

            await context.SaveChangesAsync(token);
        };
}