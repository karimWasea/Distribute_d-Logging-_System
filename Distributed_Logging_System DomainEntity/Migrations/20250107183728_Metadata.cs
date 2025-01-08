using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Distributed_Logging_System_DomainEntity.Migrations
{
    /// <inheritdoc />
    public partial class Metadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "logEntries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Metadata",
                table: "logEntries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "logEntries");

            migrationBuilder.DropColumn(
                name: "Metadata",
                table: "logEntries");
        }
    }
}
