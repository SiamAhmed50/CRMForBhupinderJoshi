using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClientTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenseStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LicenseEndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8968c0e9-6478-454c-b185-cdb1957cf8d2", "AQAAAAIAAYagAAAAEPA/CZosbODGX5QGpZAglztYVfDZSoLKA3tk4knCU2u3ZZe3cv+t9+hfVTVJWb9zAg==", "5b99fc52-0734-4ca2-929e-39c33088c460" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "44d23962-9639-437a-a4bd-b61001cb22f7", "AQAAAAIAAYagAAAAEBLf1pXRw3zPaYriUzB8mWAaZNAnTOIBtc0thBd+km2yrq27kRphJ6LD9QdgFjj3GA==", "40615a7a-b0f6-425a-b0ae-c6239b042c57" });
        }
    }
}
