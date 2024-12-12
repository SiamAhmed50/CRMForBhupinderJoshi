using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMachineEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Machines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    MachineIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Machines_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0a54c43c-3172-4786-b0a9-b0b2f52fdec4", "AQAAAAIAAYagAAAAEA+S+bR0F/X5ELxhaRP+1pOI4J6ulzQC8xYz9SQAYDsQ3s9Wt2nUIkz/YUjDPtMvmg==", "fc1a870a-f3f3-4f62-a0b2-9c29b54da897" });

            migrationBuilder.CreateIndex(
                name: "IX_Machines_ClientId",
                table: "Machines",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Machines");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ac0a3682-fd21-4aec-b4d3-c9763d041de7", "AQAAAAIAAYagAAAAEM3QOcZ1UXNHtPznExsffD1SyLkmxN1akOzsoo72oVU7Dr7T+WQC3xzOxH+G4+7Mmw==", "847c82b4-596d-4b76-b9d3-fb957d22e49a" });
        }
    }
}
