using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class JobTransactionCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0f8873fc-88d0-4f3d-8f63-906198e88489", "AQAAAAIAAYagAAAAEI9jXUpPeS0P+EOdfY3poNpyMc++etCGDEJgyvuLqAWcJnN7L4YwXW0G9PNttQ4urg==", "58f2188b-5095-46f1-afce-dad7f0751131" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4dc5f40c-0e83-47bf-bb38-757be0cb5afc", "AQAAAAIAAYagAAAAELvZB5uA8cRNpjxhnTaUoqShgv6SSwBWoQqvthAzo+smkCinKH5LcH8JwbJ1B9Sf1Q==", "f05bd69f-5155-4c9d-9b78-4adcc104a0ed" });
        }
    }
}
