using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StajEvrakSistemi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    FailedLoginCount = table.Column<int>(type: "INTEGER", nullable: false),
                    LockedUntil = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    AdSoyad = table.Column<string>(type: "TEXT", nullable: true),
                    TcKimlikNo = table.Column<string>(type: "TEXT", nullable: true),
                    Uyruk = table.Column<string>(type: "TEXT", nullable: true),
                    OgrenciNo = table.Column<string>(type: "TEXT", nullable: true),
                    DogumTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Adres = table.Column<string>(type: "TEXT", nullable: true),
                    Bolum = table.Column<string>(type: "TEXT", nullable: true),
                    OncekiStajGunSayisi = table.Column<int>(type: "INTEGER", nullable: true),
                    Telefon = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
