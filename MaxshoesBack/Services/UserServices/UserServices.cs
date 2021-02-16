using MaxshoesBack.AppDbContext;
using MaxshoesBack.Models.UserModels;
using System;
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
            context.Add(user);
            return (user);
        }

        public void Delete(User user)
        {
            var toDelete = context.Users.Where(u => u.UserName == user.UserName || u.Email == user.Email || u.Password == user.Password).FirstOrDefault();
            context.Users.Remove(toDelete);
        }

        public User Edit(User user)
        {
            context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return user;
        }

        public List<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string userEmail)
        {
            return context.Users.Where(r => r.Email == userEmail).FirstOrDefault();
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