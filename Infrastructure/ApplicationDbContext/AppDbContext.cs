using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModelsEntity;

namespace ApplicationDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json")
                            .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultDatabaseConnection"));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoleMapping> UserRoleMappings { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<ServiceCategoryMapping> ServiceCategoryMappings { get; set; }
        public DbSet<CategoryProfessionMapping> CategoryProfessionMappings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Performer> Performsers { get; set; }
    }
}
