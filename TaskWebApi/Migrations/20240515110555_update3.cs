using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskWebApi.Migrations
{
    /// <inheritdoc />
    public partial class update3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wage_AspNetUsers_UserEntityId",
                table: "Wage");

            migrationBuilder.RenameColumn(
                name: "UserEntityId",
                table: "Wage",
                newName: "UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Wage_UserEntityId",
                table: "Wage",
                newName: "IX_Wage_UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Wage_AspNetUsers_UserId1",
                table: "Wage",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wage_AspNetUsers_UserId1",
                table: "Wage");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "Wage",
                newName: "UserEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Wage_UserId1",
                table: "Wage",
                newName: "IX_Wage_UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wage_AspNetUsers_UserEntityId",
                table: "Wage",
                column: "UserEntityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
