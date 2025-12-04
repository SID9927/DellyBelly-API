using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DellyBelly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGalleryAndRefactorImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Images_ImageId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ImageId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GalleryId",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Images",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Galleries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Galleries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_CategoryId",
                table: "Images",
                column: "CategoryId",
                unique: true,
                filter: "[CategoryId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Images_GalleryId",
                table: "Images",
                column: "GalleryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Categories_CategoryId",
                table: "Images",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Galleries_GalleryId",
                table: "Images",
                column: "GalleryId",
                principalTable: "Galleries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Categories_CategoryId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Galleries_GalleryId",
                table: "Images");

            migrationBuilder.DropTable(
                name: "Galleries");

            migrationBuilder.DropIndex(
                name: "IX_Images_CategoryId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_GalleryId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "GalleryId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Images");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ImageId",
                table: "Categories",
                column: "ImageId",
                unique: true,
                filter: "[ImageId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Images_ImageId",
                table: "Categories",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
