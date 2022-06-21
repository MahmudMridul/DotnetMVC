using DotNetMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetMVC.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<ApplicationType> ApplicationTypes { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
