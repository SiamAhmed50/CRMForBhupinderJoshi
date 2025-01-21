using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class ScheduleListWeeks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4b45084e-2c2c-4d94-81d0-184b0bd4f950", "AQAAAAIAAYagAAAAEHEFCVPJTgI8zAjmPToLyRgSV/i9XvfRvFKx3RR3asGTnClrdPNZqDMW60y3qiqoug==", "061cfa93-3d53-454f-b897-9cfe555b6af0" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3db64a68-e6d1-4ae6-a8dd-b81677f8a3ed", "AQAAAAIAAYagAAAAEFXL0SZSd+m+NGtUEzMUJX5BLUMIGENZmP/mJVgoNydcjsmhDdJVe/uop7mAeD+3ZA==", "93b29feb-f313-45e5-9d3c-87f78343edfd" });
        }
    }
}
