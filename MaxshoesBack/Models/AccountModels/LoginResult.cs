using MaxshoesBack.Models.UserModels;
using System.Collections.Generic;

namespace MaxshoesBack.Controllers
{
    public class LoginResult
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string Role { get; set; }

        public string OriginalUserName { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public List<Notification> Notifications { get; set; }
    }
}