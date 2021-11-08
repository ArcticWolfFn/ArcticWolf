using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArcticWolf.Storage.Migrations
{
    public partial class AddEventFlagModificaitons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FnEventFlagModifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModifiedTimeSpanId = table.Column<int>(type: "INTEGER", nullable: true),
                    OverriddenStartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NewStartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OverriddenEndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NewEndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FnEventFlagEvent = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FnEventFlagModifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FnEventFlagModifications_FnEventFlags_FnEventFlagEvent",
                        column: x => x.FnEventFlagEvent,
                        principalTable: "FnEventFlags",
                        principalColumn: "Event",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FnEventFlagModifications_FnEventFlagTimeSpans_ModifiedTimeSpanId",
                        column: x => x.ModifiedTimeSpanId,
                        principalTable: "FnEventFlagTimeSpans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FnEventFlagModifications_FnEventFlagEvent",
                table: "FnEventFlagModifications",
                column: "FnEventFlagEvent");

            migrationBuilder.CreateIndex(
                name: "IX_FnEventFlagModifications_ModifiedTimeSpanId",
                table: "FnEventFlagModifications",
                column: "ModifiedTimeSpanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FnEventFlagModifications");
        }
    }
}
