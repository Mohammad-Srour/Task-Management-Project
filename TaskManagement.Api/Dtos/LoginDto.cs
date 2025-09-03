using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Api.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage ="Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }
    }
}
