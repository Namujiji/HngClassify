# HngClassify

A lightweight .NET 10 Web API that classifies a given first name using the Genderize service. The project exposes a simple endpoint that wraps calls to the Genderize API and returns a normalized JSON response suitable for downstream clients.

## Table of Contents
- [Project overview](#project-overview)
- [Architecture](#architecture)
- [Requirements](#requirements)
- [Configuration](#configuration)
- [Run locally](#run-locally)
- [API Usage](#api-usage)
- [Examples](#examples)
- [Testing](#testing)
- [Development notes](#development-notes)
- [Extending & Production Considerations](#extending--production-considerations)
- [Contributing](#contributing)
- [License](#license)

## Project overview

`HngClassify` provides a single purpose HTTP API to classify a first name into an inferred gender using the external Genderize API. It separates concerns between the HTTP layer, application logic, and the external service client to keep code testable and maintainable.

Core responsibilities:
- Accept a `name` parameter from clients
- Call the external Genderize API via a dedicated service client
- Normalize and return a concise JSON response

## Architecture

Key files and responsibilities:

- `Program.cs` — application startup and host configuration
- `Extensions/ApiServicesRegistration.cs` — dependency injection and service registrations
- `Controllers/ClassifyController.cs` — HTTP endpoints
- `Infrastructure/Services/GenderizeClient.cs` — HTTP client that calls the Genderize API
- `Application/Abstractions/Services/IGenderizeClient.cs` — client interface used for DI and testing
- `Application/Features/Genderize/GetGenderByName/GetGenderByNameQuery.cs` — CQRS-style query/handler for classification
- `Application/Features/Genderize/Responses/GenderizeResponse.cs` — DTO for raw Genderize response
- `Application/Features/Genderize/Responses/ClassifyResponse.cs` — DTO returned by the API
- `appsettings.json` — configuration (overridden by environment variables)

This separation makes it straightforward to mock `IGenderizeClient` for unit and integration testing.

## Requirements

- .NET 10 SDK
- Internet access to call the external Genderize service
- Optional: `GENDERIZE_API_KEY` if you use an authenticated/premium Genderize account

## Configuration

Settings may be provided in `appsettings.json` or via environment variables. The following keys are recommended:

- `GENDERIZE_BASE_URL` — base URL for the Genderize API (default: `https://api.genderize.io`)
- `GENDERIZE_API_KEY` — API key or token if required by your account

If your `ApiServicesRegistration` or `GenderizeClient` expects different configuration keys, mirror those keys in `appsettings.json` or as environment variables.

Example `appsettings.json` snippet:

```json
{
  "Genderize": {
    "BaseUrl": "https://api.genderize.io",
    "ApiKey": ""
  }
}
```

Environment variables (PowerShell example):

```powershell
$Env:GENDERIZE_BASE_URL = 'https://api.genderize.io'
$Env:GENDERIZE_API_KEY = 'your_key_here'
```

## Run locally

1. Restore and build:

```powershell
dotnet restore
dotnet build
```

2. Run the API:

```powershell
dotnet run --project .
```

3. The server URL and port will be printed in the console. Use that address to call the endpoints.

## API Usage

Endpoint:

- `GET /api/classify?name=<firstName>`

Query parameters:

- `name` (required) — first name to classify

Response (example):

```json
{
  "status": "success",
  "data": {
    "name": "mark",
    "gender": "male",
    "probability": 1,
    "sampleSize": 1378167,
    "isConfident": true,
    "processedAt": "2026-04-16T10:08:32.8926875Z"
  }
}
```

Fields:

- `status` — request status (`success` or `error`)
- `data` — response payload object
  - `name` — the queried name
  - `gender` — predicted gender (`male`, `female`, or `null`)
  - `probability` — confidence score (0..1)
  - `sampleSize` — number of records used to infer the gender
  - `isConfident` — whether the prediction is considered reliable
  - `processedAt` — ISO timestamp indicating when the request was processed

## Examples

curl:

```bash
curl "http://localhost:5000/api/classify?name=alex"
```

C# (HttpClient):

```csharp
var client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
var response = await client.GetFromJsonAsync<ClassifyResponse>("/api/classify?name=alex");
```

Replace `ClassifyResponse` with the DTO in `Application/Features/Genderize/Responses/ClassifyResponse.cs`.

## Testing

- Unit tests should mock `IGenderizeClient` and assert the query handler and controller behavior.
- Integration tests can spin up the WebApplicationFactory and mock the HTTP client using DI overrides.

Suggested test targets:

- `Application` layer handlers and mappers
- `Controllers/ClassifyController.cs` for routing and validation

## Development notes

- Register services in `Extensions/ApiServicesRegistration.cs`.
- Implement `IGenderizeClient` in `Infrastructure/Services/GenderizeClient.cs` and keep HTTP concerns isolated.
- Use `Polly` for retry/circuit-breaker policies around the external call.

## Extending & Production Considerations

- Add caching (in-memory or distributed) for repeated name lookups to reduce external calls and improve latency.
- Add rate limiting and request validation to protect the API from abuse.
- Monitor and log external API failures and add telemetry.
- Store secrets (API keys) using a secret manager or environment variables — do not commit secrets to source control.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Add tests for new behavior
4. Open a pull request against `main`

Follow the existing coding style and keep changes small and focused.

## License

This project is licensed under the MIT License — see the `LICENSE` file for details.

---

If you need the README adjusted for a different audience (API consumers, maintainers, or deploy instructions), open an issue or submit a PR with the suggested changes.
