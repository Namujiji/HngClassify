using HngClassify.Application.Abstractions.Services;
using HngClassify.Application.Features.Genderize.Responses;

namespace HngClassify.Infrastructure.Services;

public class GenderizeClient(HttpClient client) : IGenderizeClient
{
    public async Task<GenderizeResponse?> GetGenderByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var response = await client.GetAsync($"?name={name}", cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<GenderizeResponse>(cancellationToken);
        }

        return new GenderizeResponse();
    }
}