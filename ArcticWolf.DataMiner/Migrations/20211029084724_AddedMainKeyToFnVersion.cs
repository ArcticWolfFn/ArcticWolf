using Microsoft.EntityFrameworkCore.Migrations;

namespace ArcticWolf.DataMiner.Migrations
{
    public partial class AddedMainKeyToFnVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MainKey",
                table: "FnVersions",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainKey",
                table: "FnVersions");
        }
    }
}
