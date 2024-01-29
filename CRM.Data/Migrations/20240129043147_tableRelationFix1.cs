using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class tableRelationFix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Clients_ClientId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_JobLogs_JobLogsId",
                table: "Logs");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5079961a-afcd-43f6-a11f-e2f7ea0d9f72", "AQAAAAIAAYagAAAAEEDBUEgNICy22Gdz8EnXMylUlb/iXE3dvLGsBZXx7pnMiF7N+TgLwggiRIzQRjWxHg==", "f07cf8a7-56ea-4ca7-8d34-a6a7ecf5cc79" });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_TaskId",
                table: "Jobs",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Clients_ClientId",
                table: "Jobs",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Tasks_TaskId",
                table: "Jobs",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_JobLogs_JobLogsId",
                table: "Logs",
                column: "JobLogsId",
                principalTable: "JobLogs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Clients_ClientId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Tasks_TaskId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_JobLogs_JobLogsId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_TaskId",
                table: "Jobs");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c262f068-b2e2-4425-8213-a901fcbd2e59", "AQAAAAIAAYagAAAAEOnvnGiab7zaLWpp+6HL3uK7uNyoJHxOUlq8yhgynxgBJBYRdy34oCmvikFWqB0tUQ==", "bb5e5db6-7535-474b-804e-371da80a340a" });

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Clients_ClientId",
                table: "Jobs",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_JobLogs_JobLogsId",
                table: "Logs",
                column: "JobLogsId",
                principalTable: "JobLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
