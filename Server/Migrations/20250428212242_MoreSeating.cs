using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class MoreSeating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Tables_TableId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_TableId",
                table: "Seats");

            migrationBuilder.AddColumn<DateTime>(
                name: "BookedAt",
                table: "Seats",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookedByUserId",
                table: "Seats",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_BookedByUserId",
                table: "Seats",
                column: "BookedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Tables_BookedByUserId",
                table: "Seats",
                column: "BookedByUserId",
                principalTable: "Tables",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Tables_BookedByUserId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_BookedByUserId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "BookedAt",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "BookedByUserId",
                table: "Seats");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_TableId",
                table: "Seats",
                column: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Tables_TableId",
                table: "Seats",
                column: "TableId",
                principalTable: "Tables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
