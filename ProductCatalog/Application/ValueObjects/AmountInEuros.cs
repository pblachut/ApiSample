using System.Globalization;

namespace ProductCatalog.Application.ValueObjects;

public record AmountInEuros(decimal Value)
{
    public static implicit operator AmountInEuros(decimal value) => new(value);

    public static implicit operator decimal(AmountInEuros value) => value.Value;

    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}