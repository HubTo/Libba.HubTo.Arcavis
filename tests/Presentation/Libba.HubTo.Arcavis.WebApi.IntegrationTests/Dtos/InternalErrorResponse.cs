namespace Libba.HubTo.Arcavis.WebApi.IntegrationTests.Dtos;

public record InternalErrorResponse(
    string Title,
    int Status,
    string Detail,
    string TraceId
);