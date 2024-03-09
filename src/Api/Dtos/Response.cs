namespace Api.Dtos;

public class Response<T>(T data)
{
    public bool Success { get; } = true;
    public T Data { get; } = data;
}
