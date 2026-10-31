using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Pickuplay.Teams.Migrations
{
    /// <inheritdoc />
    public partial class AddCompetionTeamEntites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "competitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    OrganizerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    SportTypeId = table.Column<long>(type: "bigint", nullable: false),
                    City = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true),
                    StartRegistration = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndRegistration = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    NbrOfTeams = table.Column<int>(type: "int", nullable: false),
                    TeamSize = table.Column<int>(type: "int", nullable: false),
                    NbrOfSubs = table.Column<int>(type: "int", nullable: false),
                    Format = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    PricePlayer = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Gender = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    MinimumAge = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "longtext", nullable: true),
                    Logo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    CoverPhoto = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Referee = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Prize = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Pennies = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_competitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_competitions_sport_types_SportTypeId",
                        column: x => x.SportTypeId,
                        principalTable: "sport_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CompetitionId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false),
                    CaptainId = table.Column<int>(type: "int", nullable: true),
                    Logo = table.Column<string>(type: "longtext", nullable: true),
                    Color = table.Column<string>(type: "longtext", nullable: true),
                    Wins = table.Column<int>(type: "int", nullable: false),
                    Losses = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_teams_competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "competition_team_entries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    CompetitionId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    IsTeam = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    GuestCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Comment = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_competition_team_entries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_competition_team_entries_competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_competition_team_entries_teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_competition_team_entries_CompetitionId",
                table: "competition_team_entries",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_competition_team_entries_PlayerId_CompetitionId",
                table: "competition_team_entries",
                columns: new[] { "PlayerId", "CompetitionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_competition_team_entries_TeamId",
                table: "competition_team_entries",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_competitions_SportTypeId",
                table: "competitions",
                column: "SportTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_teams_CompetitionId_Name",
                table: "teams",
                columns: new[] { "CompetitionId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "competition_team_entries");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropTable(
                name: "competitions");
        }
    }
}
