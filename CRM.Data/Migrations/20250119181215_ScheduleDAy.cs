using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class ScheduleDAy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DayOfWeek",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3db64a68-e6d1-4ae6-a8dd-b81677f8a3ed", "AQAAAAIAAYagAAAAEFXL0SZSd+m+NGtUEzMUJX5BLUMIGENZmP/mJVgoNydcjsmhDdJVe/uop7mAeD+3ZA==", "93b29feb-f313-45e5-9d3c-87f78343edfd" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DayOfWeek",
                table: "Schedules",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "acf18f07-56bf-4986-9833-4040c2fd6221", "AQAAAAIAAYagAAAAEPFN40Mue9kqFg+vm1pKbcl7X9dOQjBh6Ikjb/YFUSQvvnH9me/9tUreRC6NW1V+zA==", "8c8be76c-193b-4729-a591-4f89fc80d16c" });
        }
    }
}
