using MaxshoesBack.AppDbContext;
using MaxshoesBack.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaxshoesBack.Services.NotificationServices
{
    public class NotificationServices : INotificationServices
    {
        private readonly ApplicationDbContext context;

        public NotificationServices(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void Complete()
        {
            context.SaveChanges();
        }

        public Notification Create(Notification Notification)
        {
            context.Notifications.Add(Notification);
            return (Notification);
        }

        public void Delete(string id)
        {
            var toDelete = context.Notifications.Where(u => u.Id==id).FirstOrDefault();
            context.Notifications.Remove(toDelete);
        }

        public Notification Edit(Notification Notification)
        {
            context.Entry(Notification).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return Notification;
        }

        public Notification Get(string id)
        {
            return context.Notifications.Where(u => u.Id == id).FirstOrDefault();
        }

        public List<Notification> GetAll()
        {
            return context.Notifications.ToList();
        }
    }
}
