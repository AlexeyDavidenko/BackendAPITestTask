using System.ComponentModel.DataAnnotations;

namespace BackendAPIDevelopmentTask.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
