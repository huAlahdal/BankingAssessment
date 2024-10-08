using System;
using System.Security.Claims;

namespace banking.Extensions;

public static class ClaimsPrincipleExtensions
{
    // Get user id from token claims
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) 
            ?? throw new Exception("Cannot get username from token"));
        
        return userId;
    }
}
