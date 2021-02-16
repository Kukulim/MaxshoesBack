using MaxshoesBack.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace MaxshoesBack.AppDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Employee1",
                    IsEmailConfirmed = true,
                    Email = "Employee1@test.pl",
                    Password = "Employee1",
                    Role = UserRoles.Employee
                },
                new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Employee2",
                    IsEmailConfirmed = true,
                    Email = "Employee2@test.pl",
                    Password = "Employee2",
                    Role = UserRoles.Employee
                }

            ); ;
        }
    }
}