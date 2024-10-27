using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SDD.Services.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class removedImageFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageLocalPath",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageLocalPath",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "ImageLocalPath", "ImageUrl" },
                values: new object[] { null, "https://placehold.co/603x403" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "ImageLocalPath", "ImageUrl" },
                values: new object[] { null, "https://placehold.co/602x402" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "ImageLocalPath", "ImageUrl" },
                values: new object[] { null, "https://placehold.co/601x401" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                columns: new[] { "ImageLocalPath", "ImageUrl" },
                values: new object[] { null, "https://placehold.co/600x400" });
        }
    }
}
