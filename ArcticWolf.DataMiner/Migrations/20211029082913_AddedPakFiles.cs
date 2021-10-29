using Microsoft.EntityFrameworkCore.Migrations;

namespace ArcticWolf.DataMiner.Migrations
{
    public partial class AddedPakFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PakFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    File = table.Column<string>(type: "TEXT", nullable: true),
                    Version1 = table.Column<decimal>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PakFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PakFiles_FnVersions_Version1",
                        column: x => x.Version1,
                        principalTable: "FnVersions",
                        principalColumn: "Version",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PakFiles_Version1",
                table: "PakFiles",
                column: "Version1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PakFiles");
        }
    }
}
