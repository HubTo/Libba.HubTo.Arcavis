namespace Libba.HubTo.Arcavis.WebApi.IntegrationTests.Dtos;

public record ValidationErrorResponse(
    string Title,
    int Status,
    Dictionary<string, string[]> Errors,
    string TraceId
);
