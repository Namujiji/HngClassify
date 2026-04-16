namespace HngClassify.Application.Features.Genderize.Responses;

public record GenderizeResponse
{
    public int Count { get; init; }
    public string? Name { get; init; }
    public string? Gender { get; init; }
    public double Probability { get; init; }
}

public interface IResult<T>
{
    bool Succeeded { get; set; }

    T? Data { get; set; }
}

public class Result<T> : IResult<T>
{
    public bool Succeeded { get; set; }

    public T? Data { get; set; }

    public static Task<Result<T>> SuccessAsync(T data)
    {
        return Task.FromResult(new Result<T>
        {
            Succeeded = true,
            Data = data
        });
    }

    public static Task<Result<T>> FailureAsync(T data)
    {
        return Task.FromResult(new Result<T>
        {
            Succeeded = false,
            Data = data
        });
    }
}