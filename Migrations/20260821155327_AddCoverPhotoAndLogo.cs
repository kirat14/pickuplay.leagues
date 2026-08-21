using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pickuplay.Teams.Migrations
{
    /// <inheritdoc />
    public partial class AddCoverPhotoAndLogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverPhoto",
                table: "Leagues",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Leagues",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverPhoto",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Leagues");
        }
    }
}
