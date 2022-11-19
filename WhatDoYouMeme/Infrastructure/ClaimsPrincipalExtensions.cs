using System.Security.Claims;
using static WhatDoYouMeme.Areas.Admin.AdminConstants;

namespace WhatDoYouMeme.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GerUserId(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.NameIdentifier).Value;

        public static bool IsAdmin(this ClaimsPrincipal user)
            => user.IsInRole(AdministratorRoleName);

        public static string GerUserEmail(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.Email).Value;
    }
}
