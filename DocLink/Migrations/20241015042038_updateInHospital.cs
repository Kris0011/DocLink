using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocLink.Migrations
{
    /// <inheritdoc />
    public partial class updateInHospital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Hospitals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Hospitals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Hospitals");
        }
    }
}
