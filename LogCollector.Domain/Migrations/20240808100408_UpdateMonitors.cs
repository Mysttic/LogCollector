using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogCollector.Domain.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMonitors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomApiCall_AuthKey",
                table: "Monitors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomApiCall_Url",
                table: "Monitors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email_Address",
                table: "Monitors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email_Subject",
                table: "Monitors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SMS_PhoneNumber",
                table: "Monitors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomApiCall_AuthKey",
                table: "Monitors");

            migrationBuilder.DropColumn(
                name: "CustomApiCall_Url",
                table: "Monitors");

            migrationBuilder.DropColumn(
                name: "Email_Address",
                table: "Monitors");

            migrationBuilder.DropColumn(
                name: "Email_Subject",
                table: "Monitors");

            migrationBuilder.DropColumn(
                name: "SMS_PhoneNumber",
                table: "Monitors");
        }
    }
}
