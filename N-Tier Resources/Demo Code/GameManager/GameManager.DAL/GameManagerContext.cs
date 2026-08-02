using GameManager.Models;
using Microsoft.EntityFrameworkCore;

namespace GameManager.DAL
{
    public class GameManagerContext : DbContext
    {
        /*~*~ DbSets ~*~*/
        public DbSet<Player> Players { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Spell> Spells { get; set; }
        /*~*~ Public Constructor ~*~*/
        public GameManagerContext(DbContextOptions<GameManagerContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*~*~ Primary Keys ~*~*/
            modelBuilder.Entity<Player>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Character>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<InventoryItem>()
                .HasKey(i => i.Id);
            modelBuilder.Entity<Spell>()
                .HasKey(s => s.Id);

            /*~*~ Properties / Constraints ~*~*/
            // Player
            modelBuilder.Entity<Player>()
                .Property(p => p.Username)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Username)
                .IsUnique();
            // Character
            modelBuilder.Entity<Character>(entity =>
            {
                entity.Property(e => e.Name);
                entity.Property(e => e.Faction);
            });
            // Inventory Item
            modelBuilder.Entity<InventoryItem>(entity =>
            {
                entity.Property(e => e.Name);
                entity.Property(e => e.Quantity);
            });
            // Spell
            modelBuilder.Entity<Spell>()
                .Property(s => s.Name);

            /*~*~ Player to Character Relationship (1:1) ~*~*/
            modelBuilder.Entity<Player>()
                .HasOne(p => p.Character)
                .WithOne(c => c.Player)
                .HasForeignKey<Character>(c => c.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            /*~*~ Character to Inventory (1:M) ~*~*/
            modelBuilder.Entity<Character>()
                .HasMany(c => c.InventoryItems)
                .WithOne(i => i.Character)
                .HasForeignKey(i => i.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            /*~*~ Character to Spell Relationship (M:N) ~*~*/
            modelBuilder.Entity<Character>()
                .HasMany(c => c.Spells)
                .WithMany(s => s.Characters);
            // Ta Da! '~'

#region ...
// Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerDatabaseCreator.v8.0.29
#endregion 
        }
    }
}
