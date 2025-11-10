using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrimeManagment.Migrations
{
    /// <inheritdoc />
    public partial class CrimeStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseParticipants_Cases_CasesCaseId",
                table: "CaseParticipants");

            migrationBuilder.DropIndex(
                name: "IX_CaseParticipants_CasesCaseId",
                table: "CaseParticipants");

            migrationBuilder.DropColumn(
                name: "CasesCaseId",
                table: "CaseParticipants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CasesCaseId",
                table: "CaseParticipants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaseParticipants_CasesCaseId",
                table: "CaseParticipants",
                column: "CasesCaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseParticipants_Cases_CasesCaseId",
                table: "CaseParticipants",
                column: "CasesCaseId",
                principalTable: "Cases",
                principalColumn: "CaseId");
        }
    }
}
