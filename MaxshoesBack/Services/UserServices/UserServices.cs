using MaxshoesBack.AppDbContext;
using MaxshoesBack.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MaxshoesBack.Services.UserServices
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext context;

        public UserServices(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Complete()
        {
            context.SaveChanges();
        }

        public User Create(User user)
        {
            context.Users.Add(user);
            return (user);
        }

        public void Delete(User user)
        {
            var toDelete = context.Users.Where(u => u.UserName == user.UserName || u.Email == user.Email || u.Password == user.Password).FirstOrDefault();
            context.Users.Remove(toDelete);
        }

        public User Edit(User user)
        {
            context.Entry(user).State = EntityState.Modified;
            return user;
        }

        public User EditEmployee(User user)
        {
            var toEditEmployee = context.Users.Where(r => r.Id == user.Id).FirstOrDefault();
            toEditEmployee.Email = user.Email;
            toEditEmployee.Password = user.Password;
            toEditEmployee.UserName = user.UserName;
            toEditEmployee.Contact = user.Contact;
            return toEditEmployee;
        }

        public List<User> GetAll()
        {
            return context.Users.ToList();
        }

        public List<User> GetAllEmployee()
        {
            return context.Users.Where(r => r.Role == UserRoles.Employee).ToList();
        }

        public User GetUserByEmail(string userEmail)
        {
            return context.Users.Where(r => r.Email == userEmail).Include(a => a.Notifications).FirstOrDefault();
        }

        public User GetUserByID(string id)
        {
            return context.Users.Where(r => r.Id == id).FirstOrDefault();
        }

        public bool IsAnExistingUser(string userName, string UserEmail)
        {
            var user = context.Users.Where(user => user.UserName == userName || user.Email == UserEmail).FirstOrDefault();
            if (user != null)
            {
                return true;
            }
            return false;
        }
    }
}