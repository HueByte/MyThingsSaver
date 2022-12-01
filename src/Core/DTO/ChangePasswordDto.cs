namespace Core.DTO
{
    public class ChangePasswordDto
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}