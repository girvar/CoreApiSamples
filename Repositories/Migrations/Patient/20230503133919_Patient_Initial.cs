using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreApiSamples.Repositories.Migrations.Patient
{
    public partial class Patient_Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Identity = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ID1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DayOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sex = table.Column<int>(type: "int", nullable: true),
                    HospitalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Identity);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ID1",
                table: "Patients",
                column: "ID1",
                unique: true,
                filter: "[ID1] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
