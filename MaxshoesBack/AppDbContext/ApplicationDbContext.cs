using MaxshoesBack.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace MaxshoesBack.AppDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}