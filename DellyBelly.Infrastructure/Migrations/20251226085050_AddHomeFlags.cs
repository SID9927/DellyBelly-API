using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DellyBelly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHomeFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Images_Galleries_GalleryId",
            //    table: "Images");

            //migrationBuilder.DropIndex(
            //    name: "IX_Images_GalleryId",
            //    table: "Images");

            //migrationBuilder.DropColumn(
            //    name: "GalleryId",
            //    table: "Images");

            //migrationBuilder.DropColumn(
            //    name: "Description",
            //    table: "Galleries");

            //migrationBuilder.DropColumn(
            //    name: "Name",
            //    table: "Galleries");

            //migrationBuilder.RenameColumn(
            //    name: "CreatedAt",
            //    table: "Galleries",
            //    newName: "UploadedAt");

            migrationBuilder.AddColumn<bool>(
                name: "IsBestSeller",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRecommended",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            //migrationBuilder.AddColumn<string>(
            //    name: "ContentType",
            //    table: "Galleries",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    defaultValue: "");

            //migrationBuilder.AddColumn<byte[]>(
            //    name: "Data",
            //    table: "Galleries",
            //    type: "varbinary(max)",
            //    nullable: false,
            //    defaultValue: new byte[0]);

            //migrationBuilder.AddColumn<string>(
            //    name: "FileName",
            //    table: "Galleries",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBestSeller",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsRecommended",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Galleries");

            migrationBuilder.RenameColumn(
                name: "UploadedAt",
                table: "Galleries",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<int>(
                name: "GalleryId",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Galleries",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Galleries",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Images_GalleryId",
                table: "Images",
                column: "GalleryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Galleries_GalleryId",
                table: "Images",
                column: "GalleryId",
                principalTable: "Galleries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
