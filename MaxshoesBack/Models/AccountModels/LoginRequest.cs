using System.ComponentModel.DataAnnotations;

namespace MaxshoesBack.Controllers
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}