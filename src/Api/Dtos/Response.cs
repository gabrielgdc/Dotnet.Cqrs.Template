namespace Api.Dtos;

/// <summary>
/// Represents a record encapsulating a response with data and a success indicator.
/// </summary>
/// <typeparam name="T">The type of data contained within the response.</typeparam>
public record Response<T>(T data)
{
    /// <summary>
    /// A boolean flag indicating the success or failure of the operation that generated the response. 
    /// Defaults to true.
    /// </summary>
    public bool Success { get; } = true;

    /// <summary>
    /// The actual data associated with the response.
    /// </summary>
    public T Data { get; } = data;
}