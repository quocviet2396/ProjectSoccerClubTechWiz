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
        public DbSet<Carts> Cart { get; set; }
        public DbSet<Categories> Category { get; set; }
        public DbSet<Products> Product { get; set; }
        public DbSet<Order> Orders { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            string str = "server=DESKTOP-T6R536I\\SQLEXPRESS01; database=MiamiDb; uid=sa; pwd=123; TrustServerCertificate=true;";
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

            modelBuilder.Entity<Carts>()
                .HasOne(c => c.Product)
                .WithMany(p => p.Carts)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Products>()
               .HasMany(p => p.Orders)
               .WithOne(o => o.Product)
               .HasForeignKey(o => o.ProductId)
               .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);


            modelBuilder.Entity<Categories>()
              .HasMany(c => c.Orders)
              .WithOne(order => order.Category)
              .HasForeignKey(order => order.CategoryId)
              .OnDelete(DeleteBehavior.NoAction);



            modelBuilder.Entity<Categories>()
                .HasMany(c => c.Carts)
                .WithOne(cart => cart.Category)
                .HasForeignKey(cart => cart.CategoryId);
        }

    }
}
