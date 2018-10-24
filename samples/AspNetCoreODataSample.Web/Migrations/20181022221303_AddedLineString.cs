using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace AspNetCoreODataSample.Web.Migrations
{
    public partial class AddedLineString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<LineString>(
                name: "LineString",
                table: "Movies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LineString",
                table: "Movies");
        }
    }
}
