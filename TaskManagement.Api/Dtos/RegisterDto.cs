using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Api.Dtos
{
    public class RegisterDto
    {

        [Required(ErrorMessage ="UserName iS Required")]
        [StringLength(50,MinimumLength =3,ErrorMessage ="UserName Must be Between 3 and 50 char")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }  
    }
}
