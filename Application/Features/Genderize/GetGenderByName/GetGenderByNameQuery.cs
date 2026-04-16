using Cortex.Mediator.Queries;
using HngClassify.Application.Abstractions.Services;
using HngClassify.Application.Features.Genderize.Responses;

namespace HngClassify.Application.Features.Genderize.GetGenderByName;

public sealed record GetGenderByNameQuery(string Name) : IQuery<Result<IClassifyResponse>>;
public sealed record GetGenderByNameQueryHandler(
    IGenderizeClient GenderizeClient,
    ILogger<GetGenderByNameQueryHandler> Logger) : IQueryHandler<GetGenderByNameQuery, Result<IClassifyResponse>>
{
    public async Task<Result<IClassifyResponse>> Handle(GetGenderByNameQuery query, CancellationToken cancellationToken)
    {
        var result = await GenderizeClient.GetGenderByNameAsync(query.Name, cancellationToken);

        if (result!.Count == 0 || string.IsNullOrWhiteSpace(result.Gender))
        {
            return await Result<IClassifyResponse>.FailureAsync(new ClassifyErrorResponse("error", "No prediction available for the provided name"));
        }

        return await Result<IClassifyResponse>.SuccessAsync(new ClassifyResponse("success", new Data
        {
            Name = result.Name,
            Gender = result.Gender,
            IsConfident = result.Probability >= 0.7 && result.Count >= 100,
            Probability = result.Probability,
            ProcessedAt = DateTime.UtcNow,
            SampleSize = result.Count
        }));
    }
}