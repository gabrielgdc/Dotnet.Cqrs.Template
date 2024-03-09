using Application.Common;
using Application.Queries.SampleQuery.Dtos;
using OneOf;

namespace Application.Queries.SampleQuery;

[GenerateOneOf]
public partial class SampleQueryResult : OneOfBase<SampleQueryResponse, ValidationFailed, Error>;
