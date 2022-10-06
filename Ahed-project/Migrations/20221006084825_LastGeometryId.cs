using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ahed_project.Migrations
{
    public partial class LastGeometryId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastGeometryId",
                table: "Users",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastGeometryId",
                table: "Users");
        }
    }
}
