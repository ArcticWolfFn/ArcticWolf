using Microsoft.EntityFrameworkCore.Migrations;

namespace ArcticWolf.Storage.Migrations
{
    public partial class AddedAesKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AesKey",
                table: "PakFiles",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AesKey",
                table: "PakFiles");
        }
    }
}
