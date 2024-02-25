using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class WeatherDb_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Сloudiness",
                table: "WeatherEntities",
                newName: "Cloudiness");

            migrationBuilder.RenameColumn(
                name: "Сloudboundary",
                table: "WeatherEntities",
                newName: "Cloudboundary");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cloudiness",
                table: "WeatherEntities",
                newName: "Сloudiness");

            migrationBuilder.RenameColumn(
                name: "Cloudboundary",
                table: "WeatherEntities",
                newName: "Сloudboundary");
        }
    }
}
