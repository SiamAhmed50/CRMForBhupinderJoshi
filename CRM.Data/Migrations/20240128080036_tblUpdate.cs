using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class tblUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c1c58396-6ea8-41ae-a843-ab2609d1da61", "AQAAAAIAAYagAAAAEM0Ij2M3MUktHXq40SD4sQrXRe2/YPhg2XzBHwmOvdEWRKY4JsxxGZKlmm1QvPPf0Q==", "5c83bcd8-60dc-4dd5-bdbf-198a4a132779" });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_TaskId",
                table: "Jobs",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Tasks_TaskId",
                table: "Jobs",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Tasks_TaskId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_TaskId",
                table: "Jobs");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b3902b32-4ee0-44a3-8063-6dcb639e6623", "AQAAAAIAAYagAAAAECX8zaqSod1ILbkCJrDrCU3U8qLp19w2U30VvXcFEY2ABpAMDIqYGu29DbfuNwaM1Q==", "c2f821ee-31cc-4537-a595-8f6980b6e606" });
        }
    }
}
