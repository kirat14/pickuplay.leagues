using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pickuplay.Teams.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerCompetitionEntriesRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "PlayerId",
                table: "competition_team_entries",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_competition_team_entries_users_PlayerId",
                table: "competition_team_entries",
                column: "PlayerId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_competition_team_entries_users_PlayerId",
                table: "competition_team_entries");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "competition_team_entries",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
