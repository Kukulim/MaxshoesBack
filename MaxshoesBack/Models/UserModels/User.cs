using System.Collections.Generic;

namespace MaxshoesBack.Models.UserModels
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; } = false;

        public List<Notification> Notifications { get; set; }
    }
}