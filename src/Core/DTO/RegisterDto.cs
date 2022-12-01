using System.ComponentModel.DataAnnotations;

namespace MTS.Core.DTO
{
    public class RegisterDto
    {

        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}