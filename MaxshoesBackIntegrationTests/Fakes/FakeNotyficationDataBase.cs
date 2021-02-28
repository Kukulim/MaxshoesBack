using MaxshoesBack.Models.UserModels;
using MaxshoesBack.Services.NotificationServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MaxshoesBackIntegrationTests.Fakes
{
    public class FakeNotyficationDataBase : INotificationServices
    {
        public List<Notification> Notifications { get; set; }
        private IReadOnlyCollection<Notification> _customDefaultNotifications;

        public FakeNotyficationDataBase(IReadOnlyCollection<Notification> notification = null)
        {
            ReplaceCustomNotification(notification);
            ResetDefaultNotifications();
        }

        public void Complete()
        {
            //empty
        }

        public Notification Create(Notification notification)
        {
            Notifications.Add(notification);
            return notification;
        }

        public void Delete(string id)
        {
            //empty
        }

        public Notification Edit(Notification notification)
        {
            var toEdit = Notifications.Where(u => u.Id == notification.Id).FirstOrDefault();
            toEdit = notification;
            return toEdit;
        }

        public Notification Get(string id)
        {
            return Notifications.Where(u => u.Id == id).FirstOrDefault();
        }

        public List<Notification> GetAll()
        {
            return Notifications;
        }

        public void ReplaceCustomNotification(IReadOnlyCollection<Notification> notifications)
        {
            _customDefaultNotifications = notifications;
        }

        public void ResetDefaultNotifications(bool useCustomIfAvailable = true)
        {
            Notifications = _customDefaultNotifications is object && useCustomIfAvailable
                ? _customDefaultNotifications.ToList()
                : GetDefaultNotifications();
        }

        public static FakeNotyficationDataBase WithDefaultNotifications()
        {
            var database = new FakeNotyficationDataBase();
            database.GetDefaultNotifications();
            return database;
        }

        private List<Notification> GetDefaultNotifications() => new List<Notification>
        {
            new Notification
            {
                Id = "97846734-172e-4149-8cec-6f43d1eb3f61",
                UserId = "37846734-172e-4149-8cec-6f43d1eb3f61",
                Title="title",
                Description="deccription",
                Response = "response",
                CreatedAt = DateTime.UtcNow,
                Status = Status.newNotify
            },
            new Notification
            {
                Id = Guid.NewGuid().ToString(),
                UserId = "37846734-172e-4149-8cec-6f43d1eb3f61",
                Title="title2",
                Description="deccription2",
                Response = "response2",
                CreatedAt = DateTime.UtcNow,
                Status = Status.pending
            },
            new Notification
            {
                Id = Guid.NewGuid().ToString(),
                UserId = "37846734-172e-4149-8cec-6f43d1eb3f61",
                Title="title3",
                Description="deccription3",
                Response = "response3",
                CreatedAt = DateTime.UtcNow,
                Status = Status.closed
            },
        };
    }
}