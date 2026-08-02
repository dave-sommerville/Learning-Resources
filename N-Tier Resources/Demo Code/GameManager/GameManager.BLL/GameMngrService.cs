using GameManager.Models;

namespace GameManager.DAL
{
    public class GameMngrService
    {
        private readonly GameMngrRepository _repository;

        public GameMngrService(GameMngrRepository repository)
        {
            _repository = repository;
        }

        public void EnsureSeedData()
        {
            if (!_repository.HasData())
            {
                _repository.SeedDemoData();
            }
        }
        /*~*~ This logic actually belongs in the controller, so the viewModels can live in the presentation layer ~*~*/
        public List<CharacterOverviewViewModel> GetCharacterOverviews()
        {
            var characters = _repository.GetAllCharactersWithDetails();

            return characters.Select(c => new CharacterOverviewViewModel
            {
                CharacterId = c.Id,
                CharacterName = c.Name,
                CharacterClass = c.Faction,
                PlayerUsername = c.Player?.Username ?? "Unassigned",
                InventoryItems = c.InventoryItems
                    .Select(i => $"{i.Name} (x{i.Quantity})")
                    .ToList(),
                KnownSpells = c.Spells
                    .Select(s => $"{s.Name}")
                    .ToList()
            }).ToList();

#region ...
// Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerDatabaseCreator.v8.0.29
#endregion
        }
    }
}
