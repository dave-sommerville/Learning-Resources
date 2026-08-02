using GameManager.Models;
using Microsoft.EntityFrameworkCore;

namespace GameManager.DAL
{
    public class GameMngrRepository
    {
        private readonly GameManagerContext _context;
        public GameMngrRepository(GameManagerContext gameManagerContext)
        {
            _context = gameManagerContext;
        }
        public bool HasData()
        {
            return _context.Characters.Any();
        }

        public List<Character> GetAllCharactersWithDetails()
        {
            return _context.Characters
                .Include(c => c.Player)
                .Include(c => c.InventoryItems)
                .Include(c => c.Spells)
                .AsNoTracking()
                .ToList();
        }
        // This is only needed once per database.
        // This isn't Linq, so remember how I feel about var. 
        public void SeedDemoData()
        {
            var spellFireball = new Spell { Name = "Fireball"};
            var spellCureWounds = new Spell { Name = "Cure Wounds" };
            var spellShield = new Spell { Name = "Shield" };

            var player1 = new Player { Username = "DragonSlayer99" };
            var player2 = new Player { Username = "HealerHero" };

            var char1 = new Character
            {
                Name = "Ignis",
                Faction = "Wizard",
                Player = player1,
                InventoryItems = new List<InventoryItem>
            {
                new() { Name = "Arcane Staff", Quantity = 1 },
                new() { Name = "Spellbook", Quantity = 1 }
            },
                Spells = new List<Spell> { spellFireball, spellShield }
            };

            var char2 = new Character
            {
                Name = "Aria",
                Faction = "Cleric",
                Player = player2,
                InventoryItems = new List<InventoryItem>
            {
                new() { Name = "Mace", Quantity = 1 },
                new() { Name = "Healing Potion", Quantity = 5 }
            },
                Spells = new List<Spell> { spellCureWounds, spellShield }
            };

            _context.Characters.AddRange(char1, char2);
            _context.SaveChanges();

#region ...
// Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerDatabaseCreator.v8.0.29
#endregion
        }
    }
}
