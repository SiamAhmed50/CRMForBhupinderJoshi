using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class Cascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_ClientTasks_ClientTaskId",
                table: "Tasks");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "308f01e9-ba3a-4fc7-92b0-06b25a22d292", "AQAAAAIAAYagAAAAEBAF/QKMOReBEYkI6IunLJ6PSeBXC0Ss27FELwZa5YbHN3pwM+9av/2M6Cs2zAtuNw==", "54327c90-80f0-4571-b66d-2b01ba3067ae" });

            migrationBuilder.CreateIndex(
                name: "IX_JobTransactions_JobId",
                table: "JobTransactions",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobTransactions_Jobs_JobId",
                table: "JobTransactions",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_ClientTasks_ClientTaskId",
                table: "Tasks",
                column: "ClientTaskId",
                principalTable: "ClientTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobTransactions_Jobs_JobId",
                table: "JobTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_ClientTasks_ClientTaskId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_JobTransactions_JobId",
                table: "JobTransactions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9e302219-782b-40de-9ede-40d705e062dc", "AQAAAAIAAYagAAAAEN0SD6ze4H9Ma2L0Jd00KptUmd616sB690RSeE4xbrut1IVrH/RShox94+Pl0IQi5g==", "30aeda20-8877-461f-b060-fdb2cbb908a1" });

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_ClientTasks_ClientTaskId",
                table: "Tasks",
                column: "ClientTaskId",
                principalTable: "ClientTasks",
                principalColumn: "Id");
        }
    }
}
