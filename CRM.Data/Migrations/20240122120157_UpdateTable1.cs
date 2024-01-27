using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                values: new object[] { "2e189e00-d6b0-46e0-b11d-791caceed137", "AQAAAAIAAYagAAAAEGJhjpjtk+7Vff4p6C5W4idK+TO13SBD+k+bCNqvYoTWiLH8B5syxlXYJDDLJ0GIxA==", "2b0fd148-64b9-4cc2-9c93-bfe972621afd" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Tasks");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a8e9e77b-2b04-4a43-a3ca-ff65bf5ceeda", "AQAAAAIAAYagAAAAECmCTc2el4EE4H4r05gUCRKusSvZlnf71t7FvLVZJTlAZL30+d052sDC9AKlFM/uwQ==", "f642de72-4a0d-4caa-89a6-57d9ed229e81" });
        }
    }
}
