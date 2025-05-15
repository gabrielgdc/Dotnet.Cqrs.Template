using Application.Features.SampleQuery.Models;
using Application.Shared.ResultTypes;
using OneOf;
using OneOf.Types;
using ResultTypes_Error = Application.Shared.ResultTypes.Error;

namespace Application.Features.SampleQuery;

/// <summary>
/// Represents a type that can hold one of the following possible response types for a SampleQuery:
/// - SampleQueryResponse: Contains the successful query results.
/// - ValidationFailed: Represents validation errors encountered during query execution.
/// - Error: Represents unexpected errors that occurred while processing the query.
/// </summary>
[GenerateOneOf]
public partial class SampleQueryResults : OneOfBase<SampleQueryResponse, NotFound, ValidationFailed, ResultTypes_Error>;