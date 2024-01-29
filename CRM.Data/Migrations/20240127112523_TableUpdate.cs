using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class TableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Jobs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Jobs",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b3902b32-4ee0-44a3-8063-6dcb639e6623", "AQAAAAIAAYagAAAAECX8zaqSod1ILbkCJrDrCU3U8qLp19w2U30VvXcFEY2ABpAMDIqYGu29DbfuNwaM1Q==", "c2f821ee-31cc-4537-a595-8f6980b6e606" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Jobs",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Jobs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bba78580-3b42-43d6-9972-ac6c8b4ed811", "AQAAAAIAAYagAAAAEBqnEVWhcOfSQ0V274FJOt8vZnMGWG6OG006hBOJus1njnp0zkntjp05qao2MCkWvA==", "b08f4bd0-a345-4c57-84b8-e37be94d807e" });
        }
    }
}
