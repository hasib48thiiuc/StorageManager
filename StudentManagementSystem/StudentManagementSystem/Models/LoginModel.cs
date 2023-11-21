using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace   StudentManagementSystem.Models
{
    public class LoginModel
    {
    
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string? UserId { get; set; }
        public IList<AuthenticationScheme>? ExternalLogins { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
