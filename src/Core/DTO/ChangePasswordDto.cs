namespace Core.DTO
{
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }
}