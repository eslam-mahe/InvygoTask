using StaffSchedulingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace StaffSchedulingSystem
{
    public class ApplicationDbContext : DbContext
    {
        //private readonly IMongoDatabase _database;

        //public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        //public IMongoCollection<Schedule> Schedules => _database.GetCollection<Schedule>("Schedules");
        //public IMongoCollection<Role> Roles => _database.GetCollection<Role>("Roles");
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IConfiguration configuration) : base(options)
        {
            //var connectionString = configuration.GetConnectionString("MongoDBConnection");
            //var client = new MongoClient(connectionString);
            //_database = client.GetDatabase("test_db");
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SqlServerConnection") + "TrustServerCertificate=true");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);
            
            modelBuilder.Entity<User>()
           .Property(u => u.Id)
           .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
           .Property(u => u.ScheduleId)
           .IsRequired(false);

            modelBuilder.Entity<Schedule>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Schedule>()
           .Property(u => u.Id)
           .ValueGeneratedOnAdd();

            modelBuilder.Entity<Role>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Role>()
           .Property(u => u.Id)
           .ValueGeneratedOnAdd();

            modelBuilder.Entity<Role>()
            .HasIndex(r => r.Name)
            .IsUnique();          

            // Add default roles to the roles table
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Staff" }
            );



            modelBuilder.Entity<User>()
                  .HasOne(u => u.Role)                      
                  .WithMany(r => r.Users)                  
                  .HasForeignKey(u => u.RoleId)          
                  .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<User>()
            //    .HasMany<Schedule>(u => u.Schedules)                  
            //    .WithOne(r => r.User)                  
            //    .HasForeignKey(u => u.UserId)            
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.User)                     
                .WithMany(u => u.Schedules)             
                .HasForeignKey(s => s.UserId)            
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
