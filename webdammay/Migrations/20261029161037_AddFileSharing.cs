using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webdammay.Migrations
{
    /// <inheritdoc />
    public partial class AddFileSharing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsShared",
                table: "StoredFiles",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ShareToken",
                table: "StoredFiles",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsShared",
                table: "StoredFiles");

            migrationBuilder.DropColumn(
                name: "ShareToken",
                table: "StoredFiles");
        }
    }
}
