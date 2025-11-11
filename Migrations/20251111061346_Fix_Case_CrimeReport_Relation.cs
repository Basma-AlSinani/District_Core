using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrimeManagment.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Case_CrimeReport_Relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseReports");

            migrationBuilder.AddColumn<int>(
                name: "CrimeReportsCrimeReportId",
                table: "Evidences",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CaseId",
                table: "CrimeReports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CrimeReportsCrimeReportId",
                table: "CaseParticipants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_CrimeReportsCrimeReportId",
                table: "Evidences",
                column: "CrimeReportsCrimeReportId");

            migrationBuilder.CreateIndex(
                name: "IX_CrimeReports_CaseId",
                table: "CrimeReports",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseParticipants_CrimeReportsCrimeReportId",
                table: "CaseParticipants",
                column: "CrimeReportsCrimeReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseParticipants_CrimeReports_CrimeReportsCrimeReportId",
                table: "CaseParticipants",
                column: "CrimeReportsCrimeReportId",
                principalTable: "CrimeReports",
                principalColumn: "CrimeReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_CrimeReports_Cases_CaseId",
                table: "CrimeReports",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "CaseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Evidences_CrimeReports_CrimeReportsCrimeReportId",
                table: "Evidences",
                column: "CrimeReportsCrimeReportId",
                principalTable: "CrimeReports",
                principalColumn: "CrimeReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseParticipants_CrimeReports_CrimeReportsCrimeReportId",
                table: "CaseParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_CrimeReports_Cases_CaseId",
                table: "CrimeReports");

            migrationBuilder.DropForeignKey(
                name: "FK_Evidences_CrimeReports_CrimeReportsCrimeReportId",
                table: "Evidences");

            migrationBuilder.DropIndex(
                name: "IX_Evidences_CrimeReportsCrimeReportId",
                table: "Evidences");

            migrationBuilder.DropIndex(
                name: "IX_CrimeReports_CaseId",
                table: "CrimeReports");

            migrationBuilder.DropIndex(
                name: "IX_CaseParticipants_CrimeReportsCrimeReportId",
                table: "CaseParticipants");

            migrationBuilder.DropColumn(
                name: "CrimeReportsCrimeReportId",
                table: "Evidences");

            migrationBuilder.DropColumn(
                name: "CaseId",
                table: "CrimeReports");

            migrationBuilder.DropColumn(
                name: "CrimeReportsCrimeReportId",
                table: "CaseParticipants");

            migrationBuilder.CreateTable(
                name: "CaseReports",
                columns: table => new
                {
                    CaseReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseId = table.Column<int>(type: "int", nullable: false),
                    CrimeReportId = table.Column<int>(type: "int", nullable: false),
                    PerformedBy = table.Column<int>(type: "int", nullable: true),
                    CasesCaseId = table.Column<int>(type: "int", nullable: true),
                    CrimeReportsCrimeReportId = table.Column<int>(type: "int", nullable: true),
                    LinkedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseReports", x => x.CaseReportId);
                    table.ForeignKey(
                        name: "FK_CaseReports_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "CaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseReports_Cases_CasesCaseId",
                        column: x => x.CasesCaseId,
                        principalTable: "Cases",
                        principalColumn: "CaseId");
                    table.ForeignKey(
                        name: "FK_CaseReports_CrimeReports_CrimeReportId",
                        column: x => x.CrimeReportId,
                        principalTable: "CrimeReports",
                        principalColumn: "CrimeReportId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseReports_CrimeReports_CrimeReportsCrimeReportId",
                        column: x => x.CrimeReportsCrimeReportId,
                        principalTable: "CrimeReports",
                        principalColumn: "CrimeReportId");
                    table.ForeignKey(
                        name: "FK_CaseReports_Users_PerformedBy",
                        column: x => x.PerformedBy,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseReports_CaseId_CrimeReportId",
                table: "CaseReports",
                columns: new[] { "CaseId", "CrimeReportId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaseReports_CasesCaseId",
                table: "CaseReports",
                column: "CasesCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseReports_CrimeReportId",
                table: "CaseReports",
                column: "CrimeReportId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseReports_CrimeReportsCrimeReportId",
                table: "CaseReports",
                column: "CrimeReportsCrimeReportId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseReports_PerformedBy",
                table: "CaseReports",
                column: "PerformedBy");
        }
    }
}
