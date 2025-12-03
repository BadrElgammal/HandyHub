using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandyHub.Migrations
{
    /// <inheritdoc />
    public partial class AddPathOfImagesProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImagePath",
                table: "Workers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImagePath",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImagePath",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImagePath",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "ProfileImagePath",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ProfileImagePath",
                table: "Admins");
        }
    }
}
