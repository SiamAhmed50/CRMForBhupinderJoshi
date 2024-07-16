using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedcolumntotasktable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_ClientTasks_ClientTaskId",
                table: "Tasks");

            migrationBuilder.AlterColumn<int>(
                name: "ClientTaskId",
                table: "Tasks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8f8a4b7f-5cd0-45cb-8e1f-40bedb839b9d", "AQAAAAIAAYagAAAAENi3/NzvC4jPJQZoVqcBvbXT9FSAEC90d2Dg3w4En6xkYCGiWtIcDJ1KECCG4KWjRg==", "7bdd8149-0481-4e7c-9888-0b87bc8bb3f5" });

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_ClientTasks_ClientTaskId",
                table: "Tasks",
                column: "ClientTaskId",
                principalTable: "ClientTasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_ClientTasks_ClientTaskId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Tasks");

            migrationBuilder.AlterColumn<int>(
                name: "ClientTaskId",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "842846f5-1024-471c-9fea-09902d2206aa", "AQAAAAIAAYagAAAAEAbRic41dC9U7M8I8oAiejJXBoaZS+UCeNu5rR8m9yumSjLPLzAJmGIG5CK7h/sMvg==", "bb94e22f-5a20-487c-93ae-ae49000ab282" });

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_ClientTasks_ClientTaskId",
                table: "Tasks",
                column: "ClientTaskId",
                principalTable: "ClientTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
