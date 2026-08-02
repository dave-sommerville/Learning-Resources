namespace GameManager.Models
{
    public class CharacterOverviewViewModel
    {
        public int CharacterId { get; set; }
        public string CharacterName { get; set; } = string.Empty;
        public string CharacterClass { get; set; } = string.Empty;

        // Flattened 1:1 Player Data
        public string PlayerUsername { get; set; } = string.Empty;

        // Flattened 1:N Inventory Data (e.g. "Arcane Staff (x1)")
        public List<string> InventoryItems { get; set; } = new List<string>();

        // Flattened N:M Spells Data (e.g. "Fireball (Lvl 3)")
        public List<string> KnownSpells { get; set; } = new List<string>();
    }
}
