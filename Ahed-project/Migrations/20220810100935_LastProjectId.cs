using Microsoft.EntityFrameworkCore.Migrations;

namespace Ahed_project.Migrations
{
    public partial class LastProjectId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastProjectId",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastProjectId",
                table: "Users");
        }
    }
}
