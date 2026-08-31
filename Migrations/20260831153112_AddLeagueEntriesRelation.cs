using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pickuplay.Teams.Migrations
{
    /// <inheritdoc />
    public partial class AddLeagueEntriesRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "league_team_entries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_league_team_entries_LeagueId",
                table: "league_team_entries",
                column: "LeagueId");

            migrationBuilder.AddForeignKey(
                name: "FK_league_team_entries_leagues_LeagueId",
                table: "league_team_entries",
                column: "LeagueId",
                principalTable: "leagues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_league_team_entries_leagues_LeagueId",
                table: "league_team_entries");

            migrationBuilder.DropIndex(
                name: "IX_league_team_entries_LeagueId",
                table: "league_team_entries");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "league_team_entries");
        }
    }
}
