namespace HngClassify.Application.Features.Genderize.Responses;

public record ClassifyResponse(
    string Status,
    Data Data
) : IClassifyResponse;

public record ClassifyErrorResponse(
    string Status,
    string Message
) : IClassifyResponse;

public interface IClassifyResponse
{
    string Status { get; init; }
}

    public record Data
{
    public string? Name { get; init; }
    public string? Gender { get; init; }
    public double Probability { get; init; }
    public int SampleSize { get; init; }
    public bool IsConfident { get; init; }
    public DateTime ProcessedAt { get; init; }
}