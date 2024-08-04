using System.ComponentModel.DataAnnotations;

namespace BackendAPIDevelopmentTask.ViewModels
{
    public class UserViewModel : LoginViewModel
    {
        [Required(ErrorMessage = "Specify if user should be with admin priveleges")]
        public bool IsAdmin { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
