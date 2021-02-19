using MaxshoesBack.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
