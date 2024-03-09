using System.Collections.Generic;

namespace Application.Queries.SampleQuery.Dtos;

public record SampleQueryResponse(IEnumerable<string> Samples);
