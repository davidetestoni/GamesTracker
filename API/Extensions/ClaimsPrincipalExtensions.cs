using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Returns the username or null if none is found.
        /// </summary>
        public static string GetUsername(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.Name)?.Value;

        /// <summary>
        /// Returns the user id or null if none is found.
        /// </summary>
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (claim is null)
            {
                return null;
            }

            return int.Parse(claim.Value);
        }
    }
}
