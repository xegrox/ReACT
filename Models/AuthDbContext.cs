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
            string connectionString = _configuration.GetConnectionString("AuthConnectionString");
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<Collection> Collections { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<CycleOfWaste> CycleOfWaste { get; set; }
        public DbSet<RecyclingType> RecyclingType{ get; set; }
        public DbSet<Reward> Reward { get; set; }
        public DbSet<RewardCategory> RewardCategory { get; set; }
        public DbSet<RewardCode> RewardCode { get; set; }
        public DbSet<RewardVariant> RewardVariant{ get; set; }
    }
}