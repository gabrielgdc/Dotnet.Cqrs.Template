using System.Collections.Generic;

namespace Application.Features.SampleQuery.Models;

/// <summary>
/// Represents a record encapsulating the response data for a sample query.
/// </summary>
public record SampleQueryResponse(IEnumerable<string> Samples);