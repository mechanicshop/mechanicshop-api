namespace MechanicShop.Domain.Common.Results;

public readonly record struct Error
{
    private Error(string code, string description, ErrorKind type)
    {
        Code = code;
        Description = description;
        Type = type;
    }

    public string Code { get; }

    public string Description { get; }

    public ErrorKind Type { get; }

    public static Error Failure(string code = nameof(Failure), string description = "General failure.")
        => new(code, description, ErrorKind.Failure);

    public static Error Validation(string code = nameof(Validation), string description = "Validation error")
        => new(code, description, ErrorKind.Validation);

    public static Error Conflict(string code = nameof(Conflict), string description = "Conflict error")
        => new(code, description, ErrorKind.Conflict);

    public static Error NotFound(string code = nameof(NotFound), string description = "Not found error")
        => new(code, description, ErrorKind.NotFound);
}
