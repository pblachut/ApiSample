namespace ProductCatalog.Application.ValueObjects;

public record Description
{
    private const int MaxDescriptionLength = 40;
    private const string DefaultDescription = "";
    
    public string Value { get; }

    private Description(string value) => Value = value;

    public static implicit operator Description(string? value) => From(value);

    public static implicit operator string(Description value) => value.Value;

    public static Description From(string? value)
    {
        return IsValidDescription()
            ? new Description(value ?? DefaultDescription)
            : throw new InvalidDescriptionException(value);
        
        bool IsValidDescription() =>
            string.IsNullOrEmpty(value)
            || value.Length < MaxDescriptionLength;
    }
        
        

    public override string ToString() => Value;
}

public class InvalidDescriptionException : Exception
{
    public InvalidDescriptionException(string? value)
        : base($"Invalid description {value}"){}
}