using ProductCatalog.Application.ValueObjects;

namespace ProductCatalog.Application.ProductRegistration;

public static class ExpectedExceptions
{
    public static bool IsExpectedException(Exception exc) =>
        exc is
            InvalidNameException or
            InvalidDescriptionException or
            InvalidRateException;
}