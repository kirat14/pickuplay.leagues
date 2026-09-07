using System;

using Microsoft.EntityFrameworkCore.Migrations;

using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Pickuplay.Teams.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLeagueToCompetitionName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_teams_leagues_LeagueId",
                table: "teams");

            migrationBuilder.DropForeignKey(
                name: "FK_league_team_entries_leagues_LeagueId",
                table: "league_team_entries");

            migrationBuilder.RenameTable(
                name: "leagues",
                newName: "competitions");

            migrationBuilder.RenameTable(
                name: "league_team_entries",
                newName: "competition_team_entries");

            migrationBuilder.RenameColumn(
                name: "LeagueId",
                table: "teams",
                newName: "CompetitionId");

            migrationBuilder.RenameIndex(
                name: "IX_teams_LeagueId_Name",
                table: "teams",
                newName: "IX_teams_CompetitionId_Name");

            migrationBuilder.RenameColumn(
                name: "LeagueId",
                table: "competition_team_entries",
                newName: "CompetitionId");

            migrationBuilder.RenameIndex(
                name: "IX_league_team_entries_LeagueId",
                table: "competition_team_entries",
                newName: "IX_competition_team_entries_CompetitionId");

            migrationBuilder.RenameIndex(
                name: "IX_league_team_entries_PlayerId_LeagueId",
                table: "competition_team_entries",
                newName: "IX_competition_team_entries_PlayerId_CompetitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_teams_competitions_CompetitionId",
                table: "teams",
                column: "CompetitionId",
                principalTable: "competitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_competition_team_entries_competitions_CompetitionId",
                table: "competition_team_entries",
                column: "CompetitionId",
                principalTable: "competitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
            name: "FK_teams_competitions_CompetitionId",
            table: "teams");

            migrationBuilder.DropForeignKey(
                name: "FK_competition_team_entries_competitions_CompetitionId",
                table: "competition_team_entries");

            migrationBuilder.RenameColumn(
                name: "CompetitionId",
                table: "teams",
                newName: "LeagueId");

            migrationBuilder.RenameIndex(
                name: "IX_teams_CompetitionId_Name",
                table: "teams",
                newName: "IX_teams_LeagueId_Name");

            migrationBuilder.RenameColumn(
                name: "CompetitionId",
                table: "competition_team_entries",
                newName: "LeagueId");

            migrationBuilder.RenameIndex(
                name: "IX_competition_team_entries_CompetitionId",
                table: "competition_team_entries",
                newName: "IX_league_team_entries_LeagueId");

            migrationBuilder.RenameIndex(
                name: "IX_competition_team_entries_PlayerId_CompetitionId",
                table: "competition_team_entries",
                newName: "IX_league_team_entries_PlayerId_LeagueId");

            migrationBuilder.RenameTable(
                name: "competition_team_entries",
                newName: "league_team_entries");

            migrationBuilder.RenameTable(
                name: "competitions",
                newName: "leagues");

            migrationBuilder.AddForeignKey(
                name: "FK_teams_leagues_LeagueId",
                table: "teams",
                column: "LeagueId",
                principalTable: "leagues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_league_team_entries_leagues_LeagueId",
                table: "league_team_entries",
                column: "LeagueId",
                principalTable: "leagues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
