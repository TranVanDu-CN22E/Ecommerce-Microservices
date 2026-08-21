namespace OrderService.Application.DTOs
{
    public sealed record GetUserDto
    {
        public string UserId { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Phone { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public bool IsLocked { get; init; }
        public bool IsBanned { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? LockedUntil { get; init; }
        public DateTime? BannedUntil { get; init; }
        public List<UserRoleDto> UserRoles { get; init; } = new();
    }
    public sealed record UserRoleDto
    {
        public string Id { get; init; } = string.Empty;
        public string UserId { get; init; } = string.Empty;
        public string RoleId { get; init; } = string.Empty;
    }
}
