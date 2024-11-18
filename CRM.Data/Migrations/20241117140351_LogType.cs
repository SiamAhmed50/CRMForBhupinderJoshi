using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class LogType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LogLevel",
                table: "Logs",
                newName: "LogType");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ac0a3682-fd21-4aec-b4d3-c9763d041de7", "AQAAAAIAAYagAAAAEM3QOcZ1UXNHtPznExsffD1SyLkmxN1akOzsoo72oVU7Dr7T+WQC3xzOxH+G4+7Mmw==", "847c82b4-596d-4b76-b9d3-fb957d22e49a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LogType",
                table: "Logs",
                newName: "LogLevel");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e83a946b-f683-484a-a667-7f2429716efa", "AQAAAAIAAYagAAAAEE739dzG9fFJ/oqiOcTK9yRZdUxW4dGqbdM630nAQYdx/zywZ8IF/zVT9FN4ekj/2w==", "501d867e-be68-4770-a83f-aad4b32a870a" });
        }
    }
}
