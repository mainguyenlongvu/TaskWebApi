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
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Wage",
                newName: "DayOffRejected");

            migrationBuilder.RenameColumn(
                name: "DayOff",
                table: "Wage",
                newName: "DayOffApproved");

            migrationBuilder.AlterColumn<double>(
                name: "Total",
                table: "Wage",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DayOffRejected",
                table: "Wage",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "DayOffApproved",
                table: "Wage",
                newName: "DayOff");

            migrationBuilder.AlterColumn<int>(
                name: "Total",
                table: "Wage",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
