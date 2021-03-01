using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaxshoesBack.Models.UserModels
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } = UserRoles.Customer;

        public bool IsEmailConfirmed { get; set; } = false;
        public Contact Contact { get; set; }

        public virtual List<Notification> Notifications { get; set; }
    }
}