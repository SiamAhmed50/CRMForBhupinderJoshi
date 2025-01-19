using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCreatedAtUpdatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Schedules");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "afaff5d5-cced-4fa2-a0f8-a3fb787076d8", "AQAAAAIAAYagAAAAEE8GvaPczck1iH38wo5jcMJ7IfPzjEWxH1ja8s/ttcqIJdW8yivutHbCBntZTcQGJQ==", "d80a3825-0182-44e2-87aa-5840d8d2beab" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Schedules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Schedules",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c8368610-4585-4583-8b2a-4c71c86cbf92", "AQAAAAIAAYagAAAAELa9AGod9OdNGzoIfGlay1+Wh26Kwm10XzPXdd4rMJThPvQmtDF1myz52HEf4T7vIw==", "279f1537-197a-47bf-aa6e-d6117d43c73c" });
        }
    }
}
