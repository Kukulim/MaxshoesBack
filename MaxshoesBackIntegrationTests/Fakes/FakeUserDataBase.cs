using MaxshoesBack.Models.UserModels;
using MaxshoesBack.Services.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using BC = BCrypt.Net.BCrypt;

namespace MaxshoesBackIntegrationTests.Fakes
{
    public class FakeUserDataBase : IUserServices
    {
        public List<User> Users { get; set; }
        private IReadOnlyCollection<User> _customDefaultUsers;

        public FakeUserDataBase(IReadOnlyCollection<User> users = null)
        {
            ReplaceCustomUsers(users);
            ResetDefaultUsers();
        }

        public void Complete()
        {
            throw new NotImplementedException();
        }

        public User Create(User user)
        {
            Users.Add(user);
            return user;
        }

        public void Delete(User user)
        {
            var toDelete = Users.Where(u => u.UserName == user.UserName || u.Email == user.Email || u.Password == user.Password).FirstOrDefault();
            Users.Remove(toDelete);
        }

        public User Edit(User user)
        {
            var toEdit = Users.Where(u => u.Email == user.Email).FirstOrDefault();
            toEdit = user;
            return toEdit;
        }

        public void EditEmployee(User user)
        {
            var toEditEmployee = Users.Where(r => r.Id == user.Id).FirstOrDefault();
            toEditEmployee.Email = user.Email;
            toEditEmployee.Password = user.Password;
            toEditEmployee.UserName = user.UserName;
            toEditEmployee.Contact = user.Contact;
        }

        public List<User> GetAll()
        {
            return Users;
        }

        public List<User> GetAllEmployee()
        {
            return Users.Where(r => r.Role == UserRoles.Employee).ToList();
        }

        public User GetUserByEmail(string userEmail)
        {
            return Users.Where(r => r.Email == userEmail).FirstOrDefault();
        }

        public User GetUserByID(string id)
        {
            return Users.Where(r => r.Id == id).FirstOrDefault();
        }

        public bool IsAnExistingUser(string userName, string UserEmail)
        {
            var user = Users.Where(user => user.UserName == userName || user.Email == UserEmail).FirstOrDefault();
            if (user != null)
            {
                return true;
            }
            return false;
        }

        public void ReplaceCustomUsers(IReadOnlyCollection<User> users)
        {
            _customDefaultUsers = users;
        }

        public void ResetDefaultUsers(bool useCustomIfAvailable = true)
        {
            Users = _customDefaultUsers is object && useCustomIfAvailable
                ? _customDefaultUsers.ToList()
                : GetDefaultUsers();
        }

        public static FakeUserDataBase WithDefaultUsers()
        {
            var database = new FakeUserDataBase();
            database.ResetDefaultUsers();
            return database;
        }

        private List<User> GetDefaultUsers() => new List<User>
        {
            new User
                {
                    Id = "37846734-172e-4149-8cec-6f43d1eb3f60",
                    UserName = "Employee1",
                    IsEmailConfirmed = true,
                    Email = "Employee1@test.pl",
                    Password = BC.HashPassword("Employee1"),
                    Role = UserRoles.Employee,
                    Contact = new Contact
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserId = "37846734-172e-4149-8cec-6f43d1eb3f60",
                            City = "Czestochowa",
                            PhoneNumber = "666111222",
                            ZipCode = "42-200",
                            ApartmentNumber = 23,
                            State = "Polska",
                            FirstName = "Piter",
                            LastName = "Blukacz",
                            HouseNumber = 45,
                            Street = "Zielona"
                        }
                },
            new User
                {
                    Id = "37846734-172e-4149-8cec-6f43d1eb3f61",
                    UserName = "Employee2",
                    IsEmailConfirmed = true,
                    Email = "Employee1@test.pl",
                    Password = BC.HashPassword("Employee2"),
                    Role = UserRoles.Employee,
                    Contact = new Contact
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserId = "37846734-172e-4149-8cec-6f43d1eb3f61",
                            City = "Krakow",
                            PhoneNumber = "662111232",
                            ZipCode = "42-202",
                            ApartmentNumber = 2,
                            State = "Polska",
                            FirstName = "Luk",
                            LastName = "Kaminski",
                            HouseNumber = 45,
                            Street = "Biala"
                        }
                },
            new User
                {
                    Id = "37846734-172e-4149-8cec-6f43d1eb3f63",
                    UserName = "TestCustomer",
                    IsEmailConfirmed = true,
                    Email = "Test@test.pl",
                    Password = BC.HashPassword("Test1234"),
                    Role = UserRoles.Customer,
                    Contact = new Contact()
                },
        };
    }
}