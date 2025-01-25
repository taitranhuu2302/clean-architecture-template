﻿using System.Security.Claims;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.API.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : IUser
{
	public string? Id => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
}