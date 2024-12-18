using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class makeClientNullForMachine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fa0664ef-715e-486d-a7fd-7cf6513b7668", "AQAAAAIAAYagAAAAEGQzOwmwuTyDyMOq9F+WDj8oVv2hJ5o/eCgPJySacRKlkzMhPqPChGMx463Da9tCUQ==", "406c3b79-cc3d-4348-920a-6ea97047ccc4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0a54c43c-3172-4786-b0a9-b0b2f52fdec4", "AQAAAAIAAYagAAAAEA+S+bR0F/X5ELxhaRP+1pOI4J6ulzQC8xYz9SQAYDsQ3s9Wt2nUIkz/YUjDPtMvmg==", "fc1a870a-f3f3-4f62-a0b2-9c29b54da897" });
        }
    }
}
