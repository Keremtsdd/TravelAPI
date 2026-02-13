using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTravels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "CountryWebs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "CountryWebs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
