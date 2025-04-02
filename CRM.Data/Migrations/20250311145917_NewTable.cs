using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5199e09e-3dd5-4e78-b43c-21c87d5a04f7", "AQAAAAIAAYagAAAAEHeIhvri61RruellixkOP1QGKYSVnv6VbIjh24B3yNWwmpkCMnpJq3nIx9BqRievTQ==", "d4b24023-cfce-493b-ae2b-a5f26e4c617d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d4c0f026-839a-4e88-b42b-a9986eeb1690", "AQAAAAIAAYagAAAAECMY8WdATwLSfLpgk3dph1SsJziNIqjxsjw6isVzoxY5pjJPP/V9bGxwwyvUI+S72w==", "b901d045-d611-4eed-8c69-7cf8a0b8c5e6" });
        }
    }
}
