using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskWebApi.Migrations
{
    /// <inheritdoc />
    public partial class update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image_Url",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "reason",
                table: "Application");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image_Url",
                table: "Application",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "reason",
                table: "Application",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
