namespace Core.DTO
{
    public class LoginLogsDto
    {
        public string Id { get; set; } = default!;
        public DateTime? LoginDate { get; set; }
        public string? IpAddress { get; set; }
        public string? Location { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
    }
}