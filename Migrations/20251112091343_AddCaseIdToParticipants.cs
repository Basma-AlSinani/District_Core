using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrimeManagment.Migrations
{
    /// <inheritdoc />
    public partial class AddCaseIdToParticipants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CaseId",
                table: "Participants",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaseId",
                table: "Participants");
        }
    }
}
