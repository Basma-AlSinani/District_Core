using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrimeManagment.Migrations
{
    /// <inheritdoc />
    public partial class AddCaseRelationsFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CrimeReports",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "CrimeReportsCrimeReportId",
                table: "CaseReports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CasesCaseId",
                table: "CaseParticipants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsersUserId",
                table: "CaseParticipants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaseReports_CrimeReportsCrimeReportId",
                table: "CaseReports",
                column: "CrimeReportsCrimeReportId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseParticipants_CasesCaseId",
                table: "CaseParticipants",
                column: "CasesCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseParticipants_UsersUserId",
                table: "CaseParticipants",
                column: "UsersUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseParticipants_Cases_CasesCaseId",
                table: "CaseParticipants",
                column: "CasesCaseId",
                principalTable: "Cases",
                principalColumn: "CaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseParticipants_Users_UsersUserId",
                table: "CaseParticipants",
                column: "UsersUserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseReports_CrimeReports_CrimeReportsCrimeReportId",
                table: "CaseReports",
                column: "CrimeReportsCrimeReportId",
                principalTable: "CrimeReports",
                principalColumn: "CrimeReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseParticipants_Cases_CasesCaseId",
                table: "CaseParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseParticipants_Users_UsersUserId",
                table: "CaseParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseReports_CrimeReports_CrimeReportsCrimeReportId",
                table: "CaseReports");

            migrationBuilder.DropIndex(
                name: "IX_CaseReports_CrimeReportsCrimeReportId",
                table: "CaseReports");

            migrationBuilder.DropIndex(
                name: "IX_CaseParticipants_CasesCaseId",
                table: "CaseParticipants");

            migrationBuilder.DropIndex(
                name: "IX_CaseParticipants_UsersUserId",
                table: "CaseParticipants");

            migrationBuilder.DropColumn(
                name: "CrimeReportsCrimeReportId",
                table: "CaseReports");

            migrationBuilder.DropColumn(
                name: "CasesCaseId",
                table: "CaseParticipants");

            migrationBuilder.DropColumn(
                name: "UsersUserId",
                table: "CaseParticipants");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CrimeReports",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }
    }
}
