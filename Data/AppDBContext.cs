using BackendAPIDevelopmentTask.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendAPIDevelopmentTask.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
