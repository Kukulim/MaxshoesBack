using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaxshoesBack.Models.UserModels
{
    public class Notification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Response { get; set; }
        public DateTime CreatedAt { get; set; }
        public Status Status { get; set; }

        public string FileUrl { get; set; }
    }
}