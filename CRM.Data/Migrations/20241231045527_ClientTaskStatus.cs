using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClientTaskStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ClientTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "09f05d58-8efa-49e0-8b5a-2d9dd445258e", "AQAAAAIAAYagAAAAEO3mm3phYz1RhanJ4wTllHH+OZXFbU1y8z9C14Mpxw7LMyTiZYXEpyuuY32kLXVMVw==", "d5293336-ffab-44d2-8ddb-9d86e8f49f32" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ClientTasks");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fa0664ef-715e-486d-a7fd-7cf6513b7668", "AQAAAAIAAYagAAAAEGQzOwmwuTyDyMOq9F+WDj8oVv2hJ5o/eCgPJySacRKlkzMhPqPChGMx463Da9tCUQ==", "406c3b79-cc3d-4348-920a-6ea97047ccc4" });
        }
    }
}
