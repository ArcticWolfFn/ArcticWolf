using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArcticWolf.Storage.Migrations
{
    public partial class AddEventFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FnEventFlags",
                columns: table => new
                {
                    Event = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FnEventFlags", x => x.Event);
                });

            migrationBuilder.CreateTable(
                name: "FnEventFlagTimeSpans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FnEventFlagEvent = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FnEventFlagTimeSpans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FnEventFlagTimeSpans_FnEventFlags_FnEventFlagEvent",
                        column: x => x.FnEventFlagEvent,
                        principalTable: "FnEventFlags",
                        principalColumn: "Event",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FnEventFlagTimeSpans_FnEventFlagEvent",
                table: "FnEventFlagTimeSpans",
                column: "FnEventFlagEvent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FnEventFlagTimeSpans");

            migrationBuilder.DropTable(
                name: "FnEventFlags");
        }
    }
}
