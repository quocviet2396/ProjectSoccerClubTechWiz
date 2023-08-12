using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectSoccerClubApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "AwayScore",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "HomeScore",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "WinnerTeam",
                table: "Match");

            migrationBuilder.AlterColumn<string>(
                name: "PlayerPosition",
                table: "Player",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PlayerNationality",
                table: "Player",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PlayerName",
                table: "Player",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "Match",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Result",
                table: "Match");

            migrationBuilder.AlterColumn<string>(
                name: "PlayerPosition",
                table: "Player",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlayerNationality",
                table: "Player",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlayerName",
                table: "Player",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Player",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AwayScore",
                table: "Match",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeScore",
                table: "Match",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WinnerTeam",
                table: "Match",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
