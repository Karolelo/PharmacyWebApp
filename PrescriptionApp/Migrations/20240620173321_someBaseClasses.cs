using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrescriptionApp.Migrations
{
    /// <inheritdoc />
    public partial class someBaseClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdPrescription",
                table: "Prescriptions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IdPatient",
                table: "Patients",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IdMedicament",
                table: "Medicaments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IdDoctor",
                table: "Doctors",
                newName: "Id");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshTokenExp = table.Column<DateTime>(type: "datetime2", nullable: true)
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

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Prescriptions",
                newName: "IdPrescription");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Patients",
                newName: "IdPatient");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Medicaments",
                newName: "IdMedicament");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Doctors",
                newName: "IdDoctor");
        }
    }
}
