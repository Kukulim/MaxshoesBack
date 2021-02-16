using MaxshoesBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaxshoesBack.Services.UserServices
{
    interface IUserServices
    {
        bool IsAnExistingUser(string userName, string UserEmail);
        public List<User> GetAll();
        public User Create(User user);
        User GetUserByEmail(string userEmail);
        User Edit(User user);
        void Delete(User user);
        void Complete();
    }
}
