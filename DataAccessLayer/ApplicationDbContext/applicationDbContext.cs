
using BussinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer.ApplicationDbContext
{
    public class applicationDbContext : DbContext
    {
        public applicationDbContext(DbContextOptions<applicationDbContext> options) : base(options)
        {

        }

        public applicationDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                IConfigurationRoot configuration = builder.Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<BatchDetail> BatchDetails { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMaterial> ProjectMaterials { get; set; }
        public DbSet<Quotation> Quotations { get; set; }
    }
}
