using System.ComponentModel.DataAnnotations;

namespace Shopping.API.DTO
{
    public class LoginFormDTO
    {
        [Required]
        [MinLength(1)]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
