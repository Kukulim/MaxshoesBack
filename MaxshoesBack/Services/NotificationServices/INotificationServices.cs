using MaxshoesBack.Models.UserModels;
using System.Collections.Generic;

namespace MaxshoesBack.Services.NotificationServices
{
    public interface INotificationServices
    {
        public List<Notification> GetAll();

        public Notification Create(Notification Notification);

        Notification Get(string id);

        Notification Edit(Notification Notification);

        void Delete(string id);

        void Complete();
    }
}