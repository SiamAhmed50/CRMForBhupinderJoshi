using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class TblUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9e302219-782b-40de-9ede-40d705e062dc", "AQAAAAIAAYagAAAAEN0SD6ze4H9Ma2L0Jd00KptUmd616sB690RSeE4xbrut1IVrH/RShox94+Pl0IQi5g==", "30aeda20-8877-461f-b060-fdb2cbb908a1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5199e09e-3dd5-4e78-b43c-21c87d5a04f7", "AQAAAAIAAYagAAAAEHeIhvri61RruellixkOP1QGKYSVnv6VbIjh24B3yNWwmpkCMnpJq3nIx9BqRievTQ==", "d4b24023-cfce-493b-ae2b-a5f26e4c617d" });
        }
    }
}
