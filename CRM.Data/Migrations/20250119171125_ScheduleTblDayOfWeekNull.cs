using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class ScheduleTblDayOfWeekNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_ClientTasks_ClientTaskId",
                table: "Schedules");

            migrationBuilder.AlterColumn<int>(
                name: "ClientTaskId",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "Schedules",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "acf18f07-56bf-4986-9833-4040c2fd6221", "AQAAAAIAAYagAAAAEPFN40Mue9kqFg+vm1pKbcl7X9dOQjBh6Ikjb/YFUSQvvnH9me/9tUreRC6NW1V+zA==", "8c8be76c-193b-4729-a591-4f89fc80d16c" });

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_ClientTasks_ClientTaskId",
                table: "Schedules",
                column: "ClientTaskId",
                principalTable: "ClientTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_ClientTasks_ClientTaskId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "Schedules");

            migrationBuilder.AlterColumn<int>(
                name: "ClientTaskId",
                table: "Schedules",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "55e7e010-2650-444e-b5f9-7ec5c6bfcd90", "AQAAAAIAAYagAAAAEK9cgQi/SK0t012acbmcGsQge4SlNqPCkO96EsmP5oq9jSGzhtpLJf5moC9LDrkZeQ==", "176a16d6-78a9-4219-b4fa-eee30dc21e7a" });

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_ClientTasks_ClientTaskId",
                table: "Schedules",
                column: "ClientTaskId",
                principalTable: "ClientTasks",
                principalColumn: "Id");
        }
    }
}
