using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.AspNetCore.Identity;

#nullable disable

namespace Distributed_Logging_System_DomainEntity.Migrations
{
    public partial class MetadataUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add a new role to the AspNetRoles table
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] {
                    "1", "Admin", "ADMIN", Guid.NewGuid().ToString()
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] {
                    "2", "User", "USER", Guid.NewGuid().ToString()
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the roles from the AspNetRoles table
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValues: new object[] { "1", "2" });
        }
    }
}
