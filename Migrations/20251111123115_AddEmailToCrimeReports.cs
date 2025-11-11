using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrimeManagment.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailToCrimeReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    ParticipantsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.ParticipantsId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SecondName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    ClearanceLevel = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    CaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    AreaCity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CaseType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AuthorizationLevel = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.CaseId);
                    table.ForeignKey(
                        name: "FK_Cases_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseAssignees",
                columns: table => new
                {
                    CaseAssigneeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CaseId = table.Column<int>(type: "int", nullable: false),
                    AssignedToUserId = table.Column<int>(type: "int", nullable: false),
                    AssignedByUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseAssignees", x => x.CaseAssigneeId);
                    table.ForeignKey(
                        name: "FK_CaseAssignees_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "CaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseAssignees_Users_AssignedByUserId",
                        column: x => x.AssignedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseAssignees_Users_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CaseComments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseComments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_CaseComments_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "CaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrimeReports",
                columns: table => new
                {
                    CrimeReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AreaCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportDataTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CrimeStatus = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CaseId = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrimeReports", x => x.CrimeReportId);
                    table.ForeignKey(
                        name: "FK_CrimeReports_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "CaseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrimeReports_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CaseParticipants",
                columns: table => new
                {
                    CaseParticipantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseId = table.Column<int>(type: "int", nullable: false),
                    ParticipantId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedByUserId = table.Column<int>(type: "int", nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CrimeReportsCrimeReportId = table.Column<int>(type: "int", nullable: true),
                    UsersUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseParticipants", x => x.CaseParticipantId);
                    table.ForeignKey(
                        name: "FK_CaseParticipants_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "CaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseParticipants_CrimeReports_CrimeReportsCrimeReportId",
                        column: x => x.CrimeReportsCrimeReportId,
                        principalTable: "CrimeReports",
                        principalColumn: "CrimeReportId");
                    table.ForeignKey(
                        name: "FK_CaseParticipants_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "ParticipantsId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseParticipants_Users_AddedByUserId",
                        column: x => x.AddedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseParticipants_Users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Evidences",
                columns: table => new
                {
                    EvidenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MimeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CaseId = table.Column<int>(type: "int", nullable: false),
                    AddedByUserId = table.Column<int>(type: "int", nullable: false),
                    CrimeReportsCrimeReportId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evidences", x => x.EvidenceId);
                    table.ForeignKey(
                        name: "FK_Evidences_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "CaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Evidences_CrimeReports_CrimeReportsCrimeReportId",
                        column: x => x.CrimeReportsCrimeReportId,
                        principalTable: "CrimeReports",
                        principalColumn: "CrimeReportId");
                    table.ForeignKey(
                        name: "FK_Evidences_Users_AddedByUserId",
                        column: x => x.AddedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvidenceAuditLogs",
                columns: table => new
                {
                    EvidenceAuditLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EvidenceAction = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    ActedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    EvidenceItemId = table.Column<int>(type: "int", nullable: false),
                    PerformedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvidenceAuditLogs", x => x.EvidenceAuditLogId);
                    table.ForeignKey(
                        name: "FK_EvidenceAuditLogs_Evidences_EvidenceItemId",
                        column: x => x.EvidenceItemId,
                        principalTable: "Evidences",
                        principalColumn: "EvidenceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvidenceAuditLogs_Users_PerformedByUserId",
                        column: x => x.PerformedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseAssignees_AssignedByUserId",
                table: "CaseAssignees",
                column: "AssignedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseAssignees_AssignedToUserId",
                table: "CaseAssignees",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseAssignees_CaseId",
                table: "CaseAssignees",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseComments_CaseId",
                table: "CaseComments",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseComments_UserId",
                table: "CaseComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseParticipants_AddedByUserId",
                table: "CaseParticipants",
                column: "AddedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseParticipants_CaseId",
                table: "CaseParticipants",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseParticipants_CrimeReportsCrimeReportId",
                table: "CaseParticipants",
                column: "CrimeReportsCrimeReportId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseParticipants_ParticipantId",
                table: "CaseParticipants",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseParticipants_UsersUserId",
                table: "CaseParticipants",
                column: "UsersUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_CreatedByUserId",
                table: "Cases",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CrimeReports_CaseId",
                table: "CrimeReports",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CrimeReports_UserId",
                table: "CrimeReports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceAuditLogs_EvidenceItemId",
                table: "EvidenceAuditLogs",
                column: "EvidenceItemId");

            migrationBuilder.CreateIndex(
                name: "IX_EvidenceAuditLogs_PerformedByUserId",
                table: "EvidenceAuditLogs",
                column: "PerformedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_AddedByUserId",
                table: "Evidences",
                column: "AddedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_CaseId",
                table: "Evidences",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_CrimeReportsCrimeReportId",
                table: "Evidences",
                column: "CrimeReportsCrimeReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseAssignees");

            migrationBuilder.DropTable(
                name: "CaseComments");

            migrationBuilder.DropTable(
                name: "CaseParticipants");

            migrationBuilder.DropTable(
                name: "EvidenceAuditLogs");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "Evidences");

            migrationBuilder.DropTable(
                name: "CrimeReports");

            migrationBuilder.DropTable(
                name: "Cases");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
