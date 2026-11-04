using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StajEvrakSistemi.Migrations
{
    /// <inheritdoc />
    public partial class AddDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", nullable: false),
                    FileType = table.Column<string>(type: "TEXT", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    OgrenciAdSoyad = table.Column<string>(type: "TEXT", nullable: false),
                    TcKimlikNo = table.Column<string>(type: "TEXT", nullable: false),
                    Uyruk = table.Column<string>(type: "TEXT", nullable: false),
                    OgretimYili = table.Column<string>(type: "TEXT", nullable: false),
                    StajBaslangic = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StajBitis = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Telefon = table.Column<string>(type: "TEXT", nullable: false),
                    Eposta = table.Column<string>(type: "TEXT", nullable: false),
                    Adres = table.Column<string>(type: "TEXT", nullable: false),
                    MuhendisSayisi = table.Column<int>(type: "INTEGER", nullable: false),
                    MuhendisBolumu = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ValidationReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DocumentId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidationReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValidationReports_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ValidationIssues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: false),
                    FieldName = table.Column<string>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    ValidationReportId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidationIssues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValidationIssues_ValidationReports_ValidationReportId",
                        column: x => x.ValidationReportId,
                        principalTable: "ValidationReports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_StudentId",
                table: "Documents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationIssues_ValidationReportId",
                table: "ValidationIssues",
                column: "ValidationReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidationReports_DocumentId",
                table: "ValidationReports",
                column: "DocumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ValidationIssues");

            migrationBuilder.DropTable(
                name: "ValidationReports");

            migrationBuilder.DropTable(
                name: "Documents");
        }
    }
}
