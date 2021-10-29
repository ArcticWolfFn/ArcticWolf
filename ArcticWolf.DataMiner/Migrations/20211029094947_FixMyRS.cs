using Microsoft.EntityFrameworkCore.Migrations;

namespace ArcticWolf.DataMiner.Migrations
{
    public partial class FixMyRS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PakFiles_FnVersions_Version1",
                table: "PakFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_TkKeys_FnVersions_Version1",
                table: "TkKeys");

            migrationBuilder.RenameColumn(
                name: "Version1",
                table: "TkKeys",
                newName: "FnVersionVersion");

            migrationBuilder.RenameIndex(
                name: "IX_TkKeys_Version1",
                table: "TkKeys",
                newName: "IX_TkKeys_FnVersionVersion");

            migrationBuilder.RenameColumn(
                name: "Version1",
                table: "PakFiles",
                newName: "FnVersionVersion");

            migrationBuilder.RenameIndex(
                name: "IX_PakFiles_Version1",
                table: "PakFiles",
                newName: "IX_PakFiles_FnVersionVersion");

            migrationBuilder.AddColumn<int>(
                name: "FnVersionId",
                table: "TkKeys",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FnVersionId",
                table: "PakFiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_PakFiles_FnVersions_FnVersionVersion",
                table: "PakFiles",
                column: "FnVersionVersion",
                principalTable: "FnVersions",
                principalColumn: "Version",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TkKeys_FnVersions_FnVersionVersion",
                table: "TkKeys",
                column: "FnVersionVersion",
                principalTable: "FnVersions",
                principalColumn: "Version",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PakFiles_FnVersions_FnVersionVersion",
                table: "PakFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_TkKeys_FnVersions_FnVersionVersion",
                table: "TkKeys");

            migrationBuilder.DropColumn(
                name: "FnVersionId",
                table: "TkKeys");

            migrationBuilder.DropColumn(
                name: "FnVersionId",
                table: "PakFiles");

            migrationBuilder.RenameColumn(
                name: "FnVersionVersion",
                table: "TkKeys",
                newName: "Version1");

            migrationBuilder.RenameIndex(
                name: "IX_TkKeys_FnVersionVersion",
                table: "TkKeys",
                newName: "IX_TkKeys_Version1");

            migrationBuilder.RenameColumn(
                name: "FnVersionVersion",
                table: "PakFiles",
                newName: "Version1");

            migrationBuilder.RenameIndex(
                name: "IX_PakFiles_FnVersionVersion",
                table: "PakFiles",
                newName: "IX_PakFiles_Version1");

            migrationBuilder.AddForeignKey(
                name: "FK_PakFiles_FnVersions_Version1",
                table: "PakFiles",
                column: "Version1",
                principalTable: "FnVersions",
                principalColumn: "Version",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TkKeys_FnVersions_Version1",
                table: "TkKeys",
                column: "Version1",
                principalTable: "FnVersions",
                principalColumn: "Version",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
