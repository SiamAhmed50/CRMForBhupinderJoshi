using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class MachineTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Machines",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "67908ee9-2fd3-4c9f-8542-dc40c0783522", "AQAAAAIAAYagAAAAENfpvQbLHevs41lpNv9hMhxMAZO+OUbyenVJSdmrxa79if7u0THMU9uWEk09RybGbg==", "b5d26ec5-6ce0-4986-8037-cd4462e38ced" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Machines");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7f7bf978-89ff-4c9f-af2c-e003b41df120", "AQAAAAIAAYagAAAAELaOo49p66SoVkb89eRdieTnJ5wO2Nmtr4LfuWFR0fmq16Q3HPYzVRpqK4qM4q0gKw==", "ab114808-e12a-4106-9708-19ebbcde1c39" });
        }
    }
}
