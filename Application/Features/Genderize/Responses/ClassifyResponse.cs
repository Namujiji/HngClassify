using System.Text.Json.Serialization;

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
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("gender")]
    public string? Gender { get; init; }

    [JsonPropertyName("probability")]
    public double Probability { get; init; }

    [JsonPropertyName("sample_size")]
    public int SampleSize { get; init; }

    [JsonPropertyName("is_confident")]
    public bool IsConfident { get; init; }

    [JsonPropertyName("processed_at")]
    public DateTime ProcessedAt { get; init; }
}