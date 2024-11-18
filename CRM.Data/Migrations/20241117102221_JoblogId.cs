using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class JoblogId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientTasks_Clients_ClientId",
                table: "ClientTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_JobLogs_Clients_ClientId",
                table: "JobLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_JobLogs_Jobs_JobId",
                table: "JobLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_JobLogs_Tasks_TaskId",
                table: "JobLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Clients_ClientId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Tasks_TasksId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_JobLogs_JoblogId",
                table: "Logs");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e83a946b-f683-484a-a667-7f2429716efa", "AQAAAAIAAYagAAAAEE739dzG9fFJ/oqiOcTK9yRZdUxW4dGqbdM630nAQYdx/zywZ8IF/zVT9FN4ekj/2w==", "501d867e-be68-4770-a83f-aad4b32a870a" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClientTasks_Clients_ClientId",
                table: "ClientTasks",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_JobLogs_Clients_ClientId",
                table: "JobLogs",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_JobLogs_Jobs_JobId",
                table: "JobLogs",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_JobLogs_Tasks_TaskId",
                table: "JobLogs",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Clients_ClientId",
                table: "Jobs",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Tasks_TasksId",
                table: "Jobs",
                column: "TasksId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_JobLogs_JoblogId",
                table: "Logs",
                column: "JoblogId",
                principalTable: "JobLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientTasks_Clients_ClientId",
                table: "ClientTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_JobLogs_Clients_ClientId",
                table: "JobLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_JobLogs_Jobs_JobId",
                table: "JobLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_JobLogs_Tasks_TaskId",
                table: "JobLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Clients_ClientId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Tasks_TasksId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_JobLogs_JoblogId",
                table: "Logs");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "489c0548-799a-41bd-8a02-fe48371d6747", "AQAAAAIAAYagAAAAEKYbpuQ452dVCuoZvY/Ql/yGvSzZVO1tG7X2oKEUTGd7yjJ5673sfTeJGKSLzFZ2YA==", "e5bb7b7b-1239-4b0e-a3bd-8293aabf2499" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClientTasks_Clients_ClientId",
                table: "ClientTasks",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobLogs_Clients_ClientId",
                table: "JobLogs",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobLogs_Jobs_JobId",
                table: "JobLogs",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobLogs_Tasks_TaskId",
                table: "JobLogs",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Clients_ClientId",
                table: "Jobs",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Tasks_TasksId",
                table: "Jobs",
                column: "TasksId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_JobLogs_JoblogId",
                table: "Logs",
                column: "JoblogId",
                principalTable: "JobLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
