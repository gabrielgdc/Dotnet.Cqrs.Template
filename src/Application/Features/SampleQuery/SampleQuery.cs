using Application.Shared.Abstracts.Queries;

namespace Application.Features.SampleQuery;

/// <summary>
/// Represents a sample query object used to retrieve data or perform an operation within the application.
/// </summary>
/// <remarks>
/// This class inherits from the `Query` base class, indicating it represents a query that returns a `SampleQueryResult` object.z
/// The `IsValid` method performs validation using a `SampleQueryValidator` and stores the validation results in the internal `ValidationResult` property.
/// </remarks>
public record SampleQuery(bool ForceClientException, string SearchTerm) : Query<SampleQueryResults>
{
    /// <summary>
    /// Can be used to simulate a client-side exception for testing purposes
    /// </summary>
    public bool ForceClientException { get; } = ForceClientException;

    public string SearchTerm { get; } = SearchTerm;

    public override bool IsValid()
    {
        ValidationResult = new SampleQueryValidator().Validate(this);
        return ValidationResult.IsValid;
    }
}