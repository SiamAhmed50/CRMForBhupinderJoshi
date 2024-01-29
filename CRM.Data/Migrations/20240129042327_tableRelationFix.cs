using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class tableRelationFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Tasks_TaskId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Jobs_JobId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_TaskId",
                table: "Jobs");

            migrationBuilder.RenameColumn(
                name: "JobId",
                table: "Logs",
                newName: "JobLogsId");

            migrationBuilder.RenameIndex(
                name: "IX_Logs_JobId",
                table: "Logs",
                newName: "IX_Logs_JobLogsId");

            migrationBuilder.CreateTable(
                name: "JobLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobLogs_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_JobLogs_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c262f068-b2e2-4425-8213-a901fcbd2e59", "AQAAAAIAAYagAAAAEOnvnGiab7zaLWpp+6HL3uK7uNyoJHxOUlq8yhgynxgBJBYRdy34oCmvikFWqB0tUQ==", "bb5e5db6-7535-474b-804e-371da80a340a" });

            migrationBuilder.CreateIndex(
                name: "IX_JobLogs_ClientId",
                table: "JobLogs",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_JobLogs_TaskId",
                table: "JobLogs",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_JobLogs_JobLogsId",
                table: "Logs",
                column: "JobLogsId",
                principalTable: "JobLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_JobLogs_JobLogsId",
                table: "Logs");

            migrationBuilder.DropTable(
                name: "JobLogs");

            migrationBuilder.RenameColumn(
                name: "JobLogsId",
                table: "Logs",
                newName: "JobId");

            migrationBuilder.RenameIndex(
                name: "IX_Logs_JobLogsId",
                table: "Logs",
                newName: "IX_Logs_JobId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c1c58396-6ea8-41ae-a843-ab2609d1da61", "AQAAAAIAAYagAAAAEM0Ij2M3MUktHXq40SD4sQrXRe2/YPhg2XzBHwmOvdEWRKY4JsxxGZKlmm1QvPPf0Q==", "5c83bcd8-60dc-4dd5-bdbf-198a4a132779" });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_TaskId",
                table: "Jobs",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Tasks_TaskId",
                table: "Jobs",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Jobs_JobId",
                table: "Logs",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
