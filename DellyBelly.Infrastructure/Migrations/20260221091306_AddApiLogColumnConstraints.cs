using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DellyBelly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddApiLogColumnConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Truncate existing data BEFORE altering columns.
            // Required because SQL Server refuses to shrink a column
            // if existing rows contain values longer than the new limit.
            migrationBuilder.Sql(@"
                UPDATE [ApiLogs] SET
                    [ResponseBody] = CASE WHEN LEN([ResponseBody]) > 4000 THEN LEFT([ResponseBody], 4000) ELSE [ResponseBody] END,
                    [RequestBody]  = CASE WHEN LEN([RequestBody])  > 4000 THEN LEFT([RequestBody],  4000) ELSE [RequestBody]  END,
                    [QueryString]  = CASE WHEN LEN([QueryString])  > 4000 THEN LEFT([QueryString],  4000) ELSE [QueryString]  END,
                    [Duration]     = CASE WHEN LEN([Duration])     > 20   THEN LEFT([Duration],     20)   ELSE [Duration]     END
                WHERE
                    LEN([ResponseBody]) > 4000
                    OR LEN([RequestBody])  > 4000
                    OR LEN([QueryString])  > 4000
                    OR LEN([Duration])     > 20;
            ");

            migrationBuilder.AlterColumn<string>(
                name: "ResponseBody",
                table: "ApiLogs",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RequestBody",
                table: "ApiLogs",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QueryString",
                table: "ApiLogs",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Duration",
                table: "ApiLogs",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ResponseBody",
                table: "ApiLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RequestBody",
                table: "ApiLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QueryString",
                table: "ApiLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Duration",
                table: "ApiLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
