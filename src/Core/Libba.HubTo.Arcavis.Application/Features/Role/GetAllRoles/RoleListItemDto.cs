﻿namespace Libba.HubTo.Arcavis.Application.Features.Role.GetAllRoles;

public class RoleListItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

