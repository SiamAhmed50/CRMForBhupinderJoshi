using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class ScheduleTblUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CronExpression",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "55e7e010-2650-444e-b5f9-7ec5c6bfcd90", "AQAAAAIAAYagAAAAEK9cgQi/SK0t012acbmcGsQge4SlNqPCkO96EsmP5oq9jSGzhtpLJf5moC9LDrkZeQ==", "176a16d6-78a9-4219-b4fa-eee30dc21e7a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CronExpression",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "afaff5d5-cced-4fa2-a0f8-a3fb787076d8", "AQAAAAIAAYagAAAAEE8GvaPczck1iH38wo5jcMJ7IfPzjEWxH1ja8s/ttcqIJdW8yivutHbCBntZTcQGJQ==", "d80a3825-0182-44e2-87aa-5840d8d2beab" });
        }
    }
}
