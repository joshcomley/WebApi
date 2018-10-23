using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace AspNetCoreODataSample.Web.Migrations
{
    public partial class AddedPolygon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Polygon>(
                name: "PolygonDb",
                table: "Movies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PolygonDb",
                table: "Movies");
        }
    }
}
