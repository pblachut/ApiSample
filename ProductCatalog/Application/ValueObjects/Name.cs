namespace ProductCatalog.Application.ValueObjects;

public record Name
{
    private const int MaxNameLength = 20;
    
    public string Value { get; }

    private Name(string value) => Value = value;

    public static implicit operator Name(string value) => From(value);

    public static implicit operator string(Name value) => value.Value;
    
    static Name From(string value) =>
        string.IsNullOrEmpty(value) || value.Length > MaxNameLength
            ? throw new InvalidNameException(value)
            : new Name(value);

    public override string ToString() => Value;
}

public class InvalidNameException : Exception
{
    public InvalidNameException(string? value)
        : base($"Invalid name: {value}"){}
}