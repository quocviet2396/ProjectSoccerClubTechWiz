using Microsoft.EntityFrameworkCore;
using ProjectModels;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Emit;


namespace ProjectSoccerClubApp.Database_Helper
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<Player> Player { get; set; }
        public DbSet<Match> Match { get; set; }
        public DbSet<Categories> Category { get; set; }
        public DbSet<Products> Product { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            string str = "server=.; database=MiamiDb; uid=sa; pwd=123456; TrustServerCertificate=true;";
            optionsBuilder.UseSqlServer(str);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Match>()
                 .HasOne(m => m.HomeTeam)
                 .WithMany()
                 .HasForeignKey(m => m.HomeTeamId)
                 .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Match>()
                .HasOne(m => m.AwayTeam)
                .WithMany()
                .HasForeignKey(m => m.AwayTeamId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Products>()
               .HasMany(p => p.OrderDetails)
               .WithOne(details => details.Product)
               .HasForeignKey(o => o.ProductId)
               .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<OrderDetails>()
                .HasOne(details => details.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(details => details.OrderId);

        }

    }
}
