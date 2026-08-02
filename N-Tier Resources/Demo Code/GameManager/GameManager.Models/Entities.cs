namespace GameManager.Models
{
    /*  
    *  No animals were harmed in the making of this project.
    *  Many coffee beans were lost though.   
    */

    /*~*~ Fantasy Game Demonstration: Stage One ~*~* 
            Entities and Relationships
            Player: 
            ~ Has One Character (1:1)
            Character: 
            ~ Has One Player (1:1)
            ~ Has Many Items (1:N)
            Item:
            ~ Has One Player (N:1)
                /*~*~*/
    public class Player
    {
        public int Id { get; set; }
        public string Username  { get; set; }

        /*~*~ 1:1 Player to Character ~*~*/
        public int? CharacterId { get; set; }
        public Character? Character { get; set; }

    }
    public class Character 
    { 
        public int Id { get; set; }
        public string Name { get; set;  } = string.Empty;
        public string Faction { get; set; }

        /*~*~ 1:1 Character to Player ~*~*/
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;

        /*~*~ 1:N Character to Items ~*~*/
        public ICollection<InventoryItem> InventoryItems { get; set; }
        /*~*~ N:M Characters to Spells ~*~*/
        public ICollection<Spell> Spells { get; set; } = new List<Spell>();

    }
    public class Spell
    {
        public int Id { get; set; }
        public string Name { get; set; }
        /*~*~ N:M Characters to Spells ~*~*/
        public ICollection<Character> Characters { get; set; }

    }
    public class InventoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        /*~*~ N:1 Navigation to the character it belongs to ~*~*/
        public int CharacterId { get; set; }
        public Character Character { get; set; } = null!;
    }
#region ...
// Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerDatabaseCreator.v8.0.29
#endregion
}
