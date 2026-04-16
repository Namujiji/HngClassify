namespace HngClassify.Configurations;

public sealed record GenderizeSettings
{
    /// <summary>
    /// Gets the base URL for Genderize API operations.
    /// </summary>
    /// <value>
    /// A string representing the root endpoint for all Genderize service requests.
    /// </value>
    public string BaseUrl { get; init; } = null!;

}