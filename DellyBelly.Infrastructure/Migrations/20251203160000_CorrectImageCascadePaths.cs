using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DellyBelly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrectImageCascadePaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Replace cascading FK from Categories -> Images with Restrict/NoAction to avoid multiple cascade paths
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Images_ImageId",
                table: "Categories");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Images_ImageId",
                table: "Categories",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert back to Cascade if the migration is rolled back
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Images_ImageId",
                table: "Categories");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Images_ImageId",
                table: "Categories",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}