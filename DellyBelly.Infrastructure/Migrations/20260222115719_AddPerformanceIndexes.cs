using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DellyBelly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_IsAvailable",
                table: "Products",
                column: "IsAvailable");

            migrationBuilder.CreateIndex(
                name: "IX_Products_IsBestSeller",
                table: "Products",
                column: "IsBestSeller");

            migrationBuilder.CreateIndex(
                name: "IX_Products_IsRecommended",
                table: "Products",
                column: "IsRecommended");

            migrationBuilder.CreateIndex(
                name: "IX_Images_Source",
                table: "Images",
                column: "Source");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_IsAvailable",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_IsBestSeller",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_IsRecommended",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Images_Source",
                table: "Images");
        }
    }
}
