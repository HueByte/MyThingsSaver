namespace Core.DTO
{
    public class UserInfoDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public DateTime AccountCreatedDate { get; set; }
        public int AccountSize { get; set; }
        public int EntriesCount { get; set; }
        public int CategoriesCount { get; set; }
        public string? AvatarUrl { get; set; }
        public string[]? Roles { get; set; }
    }
}