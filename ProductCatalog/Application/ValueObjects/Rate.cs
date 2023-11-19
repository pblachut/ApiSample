using System.Globalization;

namespace ProductCatalog.Application.ValueObjects;

public record Rate
{
    public double Value { get; }

    private Rate(double value) => Value = value;

    public static Rate From(double value)
    {
        return IsValidRate()
            ? new Rate(Math.Round(value, 1))
            : throw new InvalidRateException(value);

        bool IsValidRate() => value is >= 0 and < 10;
    }
    
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}

public class InvalidRateException : Exception
{
    public InvalidRateException(double value)
        : base($"Cant create rate with value: {value}")
    {
        
    }
}