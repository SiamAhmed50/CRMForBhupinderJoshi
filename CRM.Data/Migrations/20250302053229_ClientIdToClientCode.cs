using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClientIdToClientCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Clients",
                newName: "ClientCode");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d4c0f026-839a-4e88-b42b-a9986eeb1690", "AQAAAAIAAYagAAAAECMY8WdATwLSfLpgk3dph1SsJziNIqjxsjw6isVzoxY5pjJPP/V9bGxwwyvUI+S72w==", "b901d045-d611-4eed-8c69-7cf8a0b8c5e6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClientCode",
                table: "Clients",
                newName: "ClientId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0f8873fc-88d0-4f3d-8f63-906198e88489", "AQAAAAIAAYagAAAAEI9jXUpPeS0P+EOdfY3poNpyMc++etCGDEJgyvuLqAWcJnN7L4YwXW0G9PNttQ4urg==", "58f2188b-5095-46f1-afce-dad7f0751131" });
        }
    }
}
