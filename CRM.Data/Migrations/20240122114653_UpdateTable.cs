using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a8e9e77b-2b04-4a43-a3ca-ff65bf5ceeda", "AQAAAAIAAYagAAAAECmCTc2el4EE4H4r05gUCRKusSvZlnf71t7FvLVZJTlAZL30+d052sDC9AKlFM/uwQ==", "f642de72-4a0d-4caa-89a6-57d9ed229e81" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a49513ea-fd2d-4257-9d0d-a63497c8fee3", "AQAAAAIAAYagAAAAEHDj+Uw9QyLXnXgFCsTDgedZPb2XNbnU13iOi2o2uFkHBN4yDJnzuZJmLY8E3SJE6g==", "e38cdb0d-c57a-49a0-bfc4-f0ec1f5154bd" });
        }
    }
}
