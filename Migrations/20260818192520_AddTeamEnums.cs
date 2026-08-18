using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pickuplay.Teams.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamType",
                table: "Teams");

            migrationBuilder.AddColumn<int>(
                name: "AgeGroup",
                table: "Teams",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Teams",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeamSize",
                table: "Teams",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgeGroup",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TeamSize",
                table: "Teams");

            migrationBuilder.AddColumn<string>(
                name: "TeamType",
                table: "Teams",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
