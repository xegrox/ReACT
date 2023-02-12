using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace ReACT.Models
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration _configuration;
        
        public AuthDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("AuthConnectionString")!;
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDbFunction(typeof(AuthDbContext).GetMethod(nameof(Difference))!)
                .HasTranslation(args => new SqlFunctionExpression("DIFFERENCE", args, true, new []{false, false}, typeof(int), null));
        }

        [DbFunction("DIFFERENCE")]
        public static int Difference(string s1, string s2) => throw new Exception();

        public DbSet<Collection> Collections { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<CycleOfWaste> CycleOfWaste { get; set; }
        public DbSet<CycleOfWaste_prizes> CycleOfWaste_prizes { get; set; }
        public DbSet<RecyclingType> RecyclingType { get; set; }

        public DbSet<Reward> Rewards { get; set; }
        public DbSet<RewardCategory> RewardCategories { get; set; }
        public DbSet<RewardCode> RewardCodes { get; set; }
        public DbSet<RewardVariant> RewardVariants { get; set; }
        
        public DbSet<RewardHistory> RewardHistories { get; set; }

        public DbSet<Thread> Threads { get; set; }

        public DbSet<PointsHistory> PointsHistory { get; set; }
    }
}