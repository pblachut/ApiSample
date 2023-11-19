using ProductCatalog.Application.ValueObjects;

namespace ProductCatalog.Application.AddReview;

public static class ExpectedExceptions
{
    public static bool IsExpectedException(Exception exc) =>
        exc is InvalidRateException or NotPossibleToAddReviewException;
}