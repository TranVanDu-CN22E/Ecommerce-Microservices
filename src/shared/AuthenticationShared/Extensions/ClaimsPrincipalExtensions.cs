using System.Security.Claims;

namespace AuthenticationShared.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserIdString(this ClaimsPrincipal user)
        {
            return user.FindFirstValue("sub")
                ?? user.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        /// <summary>
        /// Lấy tất cả roles của user từ JWT token
        /// </summary>
        public static List<string> GetRoles(this ClaimsPrincipal user)
        {
            return user.FindAll(ClaimTypes.Role)
                      .Select(c => c.Value)
                      .Where(r => !string.IsNullOrEmpty(r))
                      .Distinct()
                      .ToList();
        }

        /// <summary>
        /// Kiểm tra user có role cụ thể không
        /// </summary>
        public static bool HasRole(this ClaimsPrincipal user, string role)
        {
            return user.IsInRole(role);
        }

        /// <summary>
        /// Kiểm tra user có bất kỳ role nào trong danh sách không
        /// </summary>
        public static bool HasAnyRole(this ClaimsPrincipal user, params string[] roles)
        {
            return roles.Any(role => user.IsInRole(role));
        }

        /// <summary>
        /// Kiểm tra user có tất cả roles trong danh sách không
        /// </summary>
        public static bool HasAllRoles(this ClaimsPrincipal user, params string[] roles)
        {
            return roles.All(role => user.IsInRole(role));
        }
    }
}
