using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ahed_project.Migrations
{
    /// <inheritdoc />
    public partial class DontSaveLoginAndPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "Token");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "Users",
                newName: "Password");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "TEXT",
                nullable: true);
        }
    }
}
