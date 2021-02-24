using MaxshoesBack.Models.UserModels;
using System.Collections.Generic;

namespace MaxshoesBack.Services.UserServices
{
    public interface IUserServices
    {
        bool IsAnExistingUser(string userName, string UserEmail);

        public List<User> GetAll();

        public User Create(User user);

        User GetUserByEmail(string userEmail);
        User GetUserByID(string id);
        User Edit(User user);

        void Delete(User user);

        void Complete();
    }
}