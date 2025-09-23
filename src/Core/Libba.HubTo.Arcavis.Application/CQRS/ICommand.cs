using MediatR;

namespace Libba.HubTo.Arcavis.Application.CQRS;

/// <summary>
/// Represents a command that returns a response.
/// Provides semantic clarity on top of MediatR's IRequest.
/// </summary>
public interface ICommand<out TResponse> : IRequest<TResponse> { }

public interface ICommand : IRequest { }