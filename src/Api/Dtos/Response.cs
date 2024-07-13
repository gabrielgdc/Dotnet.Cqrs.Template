namespace Api.Dtos;

/// <summary>
/// Represents a record encapsulating a response with data and a success indicator.
/// </summary>
/// <typeparam name="T">The type of data contained within the response.</typeparam>
/// <param name="Success"> A boolean flag indicating the success or failure of the operation that generated the response. Defaults to true.</param>
/// <param name="Data"> The actual data associated with the response. </param>
public record Response<T>(T Data, bool Success = true);