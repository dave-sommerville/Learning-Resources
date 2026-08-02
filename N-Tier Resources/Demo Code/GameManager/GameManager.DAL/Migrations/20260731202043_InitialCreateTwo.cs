using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateTwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterSpell_Spell_SpellsId",
                table: "CharacterSpell");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Spell",
                table: "Spell");

            migrationBuilder.RenameTable(
                name: "Spell",
                newName: "Spells");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Spells",
                table: "Spells",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterSpell_Spells_SpellsId",
                table: "CharacterSpell",
                column: "SpellsId",
                principalTable: "Spells",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterSpell_Spells_SpellsId",
                table: "CharacterSpell");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Spells",
                table: "Spells");

            migrationBuilder.RenameTable(
                name: "Spells",
                newName: "Spell");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Spell",
                table: "Spell",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterSpell_Spell_SpellsId",
                table: "CharacterSpell",
                column: "SpellsId",
                principalTable: "Spell",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
