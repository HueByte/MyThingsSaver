namespace Core.DTO
{
    public class UserInfoDto
    {
        public string? Username { get; set; }
        public DateTime AccountCreatedDate { get; set; }
        public int EntriesCount { get; set; }
        public int CategoriesCount { get; set; }
        public string? AvatarUrl { get; set; }
    }
}