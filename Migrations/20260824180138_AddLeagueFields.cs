using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pickuplay.Teams.Migrations
{
    /// <inheritdoc />
    public partial class AddLeagueFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MinTeamPlayers",
                table: "Leagues",
                newName: "TeamSize");

            migrationBuilder.RenameColumn(
                name: "MaxTeamPlayers",
                table: "Leagues",
                newName: "NbrOfSubs");

            migrationBuilder.AddColumn<bool>(
                name: "Pennies",
                table: "Leagues",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Prize",
                table: "Leagues",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Referee",
                table: "Leagues",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pennies",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "Prize",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "Referee",
                table: "Leagues");

            migrationBuilder.RenameColumn(
                name: "TeamSize",
                table: "Leagues",
                newName: "MinTeamPlayers");

            migrationBuilder.RenameColumn(
                name: "NbrOfSubs",
                table: "Leagues",
                newName: "MaxTeamPlayers");
        }
    }
}
