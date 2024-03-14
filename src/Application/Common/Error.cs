namespace Application.Common;

/// <summary>
/// Represents an error with a code, title, and detailed description.
/// </summary>
public record Error(
    string Code,
    string Title,
    string Detail
);
