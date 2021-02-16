using System;

namespace MaxshoesBack.Models.UserModels
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