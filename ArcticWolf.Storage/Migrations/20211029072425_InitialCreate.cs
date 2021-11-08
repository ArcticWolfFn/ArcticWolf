using Microsoft.EntityFrameworkCore.Migrations;

namespace ArcticWolf.Storage.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FnVersions",
                columns: table => new
                {
                    Version = table.Column<decimal>(type: "TEXT", nullable: false),
                    VersionString = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FnVersions", x => x.Version);
                });

            migrationBuilder.CreateTable(
                name: "TkKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "TEXT", nullable: true),
                    Version1 = table.Column<decimal>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TkKeys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TkKeys_FnVersions_Version1",
                        column: x => x.Version1,
                        principalTable: "FnVersions",
                        principalColumn: "Version",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TkKeys_Version1",
                table: "TkKeys",
                column: "Version1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TkKeys");

            migrationBuilder.DropTable(
                name: "FnVersions");
        }
    }
}
