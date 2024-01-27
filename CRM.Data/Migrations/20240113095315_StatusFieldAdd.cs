using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class StatusFieldAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LicenseStatus",
                table: "Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "15f0f183-dd1c-4b79-83c8-5bd4335483fa", "AQAAAAIAAYagAAAAEHrkhURlT26fznszJhzY6TyQ0q34I9JV7pZlVBaBNoolmJIy4C5Fy80YfyFzwqlaSw==", "23fee235-b2cb-4a08-99a5-7d441bc9fbe5" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicenseStatus",
                table: "Clients");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8968c0e9-6478-454c-b185-cdb1957cf8d2", "AQAAAAIAAYagAAAAEPA/CZosbODGX5QGpZAglztYVfDZSoLKA3tk4knCU2u3ZZe3cv+t9+hfVTVJWb9zAg==", "5b99fc52-0734-4ca2-929e-39c33088c460" });
        }
    }
}
