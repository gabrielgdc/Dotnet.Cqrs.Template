namespace Application.Queries.SampleQuery;

public class SampleQuery(bool forceClientException) : Query<SampleQueryResult>
{
    public bool ForceClientException { get; } = forceClientException;

    public override bool IsValid()
    {
        ValidationResult = new SampleQueryValidator().Validate(this);
        return ValidationResult.IsValid;
    }
}
