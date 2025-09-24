using Libba.HubTo.Arcavis.Application.Services.Endpoint.Commands;
using FluentValidation;

namespace Libba.HubTo.Arcavis.Application.Services.Endpoint.Validators;

public class CreateEndpointCommandValidator : AbstractValidator<CreateEndpointCommand>
{
    public CreateEndpointCommandValidator()
    {
        
    }
}
