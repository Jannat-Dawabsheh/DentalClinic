using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalClinic.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateindex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorId_StartDateTime",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId_StartDateTime",
                table: "Appointments",
                columns: new[] { "DoctorId", "StartDateTime" },
                unique: true,
                filter: "[Status] IN (0,1)");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId_StartDateTime",
                table: "Appointments",
                columns: new[] { "PatientId", "StartDateTime" },
                unique: true,
                filter: "[Status] IN (0,1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorId_StartDateTime",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_PatientId_StartDateTime",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId_StartDateTime",
                table: "Appointments",
                columns: new[] { "DoctorId", "StartDateTime" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");
        }
    }
}
