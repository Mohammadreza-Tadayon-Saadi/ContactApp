﻿using System.Globalization;
using System.Security.Claims;
using System.Security.Principal;

namespace Core.Utilities;

public static class IdentityExtentions
{
    public static string FindFirstValue(this ClaimsIdentity identity, string claimType)
    {
        return identity?.FindFirst(claimType)?.Value;
    }

    public static string FindFirstValue(this IIdentity identity , string claimType)
    {
        var claimsIdentity = identity as ClaimsIdentity;
        return claimsIdentity?.FindFirstValue(claimType);
    }

    public static string GetUserId(this IIdentity identity)
    {
        return identity?.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public static T GetUserId<T>(this IIdentity identity) where T : IConvertible
    {
        string? userId = identity?.GetUserId();
        return userId != null ? (T)Convert.ChangeType(userId, typeof(T), CultureInfo.InvariantCulture)
                                 : default;
    }

    public static string GetUserName(this IIdentity identity)
    {
        return identity?.FindFirstValue(ClaimTypes.Name);
    }

    public static string GetRoles(this IIdentity identity)
    {
        return identity?.FindFirstValue(ClaimTypes.Role);
    }
}