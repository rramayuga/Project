using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class InitMySQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0ff4cd29-9160-4378-9ca7-ed4afa9226b7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cf46adb4-49f2-4e5c-a66e-63ce35c60a39");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7d4d461c-afdb-4705-a602-07c222635ecd", null, "Admin", "ADMIN" },
                    { "b24a7560-4f91-4270-a394-ef10db8b6f0f", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7d4d461c-afdb-4705-a602-07c222635ecd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b24a7560-4f91-4270-a394-ef10db8b6f0f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0ff4cd29-9160-4378-9ca7-ed4afa9226b7", null, "Admin", "ADMIN" },
                    { "cf46adb4-49f2-4e5c-a66e-63ce35c60a39", null, "User", "USER" }
                });
        }
    }
}
