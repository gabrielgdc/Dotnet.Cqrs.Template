namespace Api.Dtos;

public record Response<T>(T data)
{
    public bool Success { get; } = true;
    public T Data { get; } = data;
}
