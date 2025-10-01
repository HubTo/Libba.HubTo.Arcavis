using Libba.HubTo.Arcavis.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libba.HubTo.Arcavis.Application.Features.User.SignIn;

public record SignInCommand
(
    string PhoneCode
) : ICommand<SignInResponseDto>;
