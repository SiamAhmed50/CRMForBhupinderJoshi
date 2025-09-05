using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class cascadeDeletes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        { 
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b39337b9-d0dd-41d4-b497-7cf8098372b3", "AQAAAAIAAYagAAAAEF+1X5Q4vR0uVVwDjW27sPa2FHhLmPHp2M3dpM4QI0qnk2LNxcMlBBXBVg7nOLjpEQ==", "ec85d281-285d-493a-9aa1-f7c9272fdbf6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        { 
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7f7bf978-89ff-4c9f-af2c-e003b41df120", "AQAAAAIAAYagAAAAELaOo49p66SoVkb89eRdieTnJ5wO2Nmtr4LfuWFR0fmq16Q3HPYzVRpqK4qM4q0gKw==", "ab114808-e12a-4106-9708-19ebbcde1c39" });
        }
    }
}
