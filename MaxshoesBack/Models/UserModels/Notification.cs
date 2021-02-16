using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaxshoesBack.Models.UserModels
{
    public class Notification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string UserId { get; set; }
        public int MyProperty { get; set; }
        public DateTime CreatedAt { get; set; }
        public Status Status { get; set; }
    }
}