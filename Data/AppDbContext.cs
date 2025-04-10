using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DragonBallApi.Models;
namespace DragonBallApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Transformation> Transformations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación uno a muchos entre Character y Transformations
            modelBuilder
                .Entity<Character>()
                .HasMany(c => c.Transformations)
                .WithOne(t => t.Character)
                .HasForeignKey(t => t.CharacterId);
        }
    }
}
