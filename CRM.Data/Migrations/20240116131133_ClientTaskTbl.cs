using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClientTaskTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientTasks_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientTaskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_ClientTasks_ClientTaskId",
                        column: x => x.ClientTaskId,
                        principalTable: "ClientTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a49513ea-fd2d-4257-9d0d-a63497c8fee3", "AQAAAAIAAYagAAAAEHDj+Uw9QyLXnXgFCsTDgedZPb2XNbnU13iOi2o2uFkHBN4yDJnzuZJmLY8E3SJE6g==", "e38cdb0d-c57a-49a0-bfc4-f0ec1f5154bd" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientTasks_ClientId",
                table: "ClientTasks",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ClientTaskId",
                table: "Tasks",
                column: "ClientTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "ClientTasks");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "15f0f183-dd1c-4b79-83c8-5bd4335483fa", "AQAAAAIAAYagAAAAEHrkhURlT26fznszJhzY6TyQ0q34I9JV7pZlVBaBNoolmJIy4C5Fy80YfyFzwqlaSw==", "23fee235-b2cb-4a08-99a5-7d441bc9fbe5" });
        }
    }
}
