using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ReACT.Models
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration _configuration;
        //public AuthDbContext(DbContextOptions<AuthDbContext> options):base(options){ }
        public AuthDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("AuthConnectionString")!;
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<Collection> Collections { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<CycleOfWaste> CycleOfWaste { get; set; }
        public DbSet<RecyclingType> RecyclingType { get; set; }

        public DbSet<Reward> Rewards { get; set; }
        public DbSet<RewardCategory> RewardCategories { get; set; }
        public DbSet<RewardCode> RewardCodes { get; set; }
        public DbSet<RewardVariant> RewardVariants { get; set; }

        public DbSet<Thread> Threads { get; set; }
    }
}