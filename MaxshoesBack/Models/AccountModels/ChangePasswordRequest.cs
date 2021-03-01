using System.ComponentModel.DataAnnotations;

namespace MaxshoesBack.Models.AccountModels
{
    public class ChangePasswordRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}