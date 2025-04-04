using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class MenuTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Menus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserMenus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMenus_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMenus_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8a00fc5a-8fd1-4bd8-abdf-699cbc60dbdc", "AQAAAAIAAYagAAAAEGYlrb+07n8W4xnLCVMwKucfEiv0uWbhjq99ueKR3N7yU+Hzpwwq99t4wTur80ehXQ==", "80b1482b-bf04-4c33-bd74-af6e32adc692" });

            migrationBuilder.CreateIndex(
                name: "IX_UserMenus_MenuId",
                table: "UserMenus",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMenus_UserId",
                table: "UserMenus",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMenus");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Menus");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "308f01e9-ba3a-4fc7-92b0-06b25a22d292", "AQAAAAIAAYagAAAAEBAF/QKMOReBEYkI6IunLJ6PSeBXC0Ss27FELwZa5YbHN3pwM+9av/2M6Cs2zAtuNw==", "54327c90-80f0-4571-b66d-2b01ba3067ae" });
        }
    }
}
