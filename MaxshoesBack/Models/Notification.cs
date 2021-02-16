using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaxshoesBack.Models
{
    public class Notification
    {
        public string Id { get; set; }

        public String UserId { get; set; }
        public int MyProperty { get; set; }
        public DateTime CreatedAt { get; set; }
        public Status Status { get; set; }
    }
}
