namespace Core.DTO
{
    public class ManagementUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string? Username { get; set; }
        public string? Email { get; set; }
        public int AccountSize { get; set; }
        public string[]? Roles { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime AccountCreatedDate { get; set; }
    }
}