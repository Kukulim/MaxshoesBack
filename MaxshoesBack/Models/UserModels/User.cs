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

        public bool IsEmailConfirmed { get; set; } = false;

        public virtual List<Notification> Notifications { get; set; }
    }
}