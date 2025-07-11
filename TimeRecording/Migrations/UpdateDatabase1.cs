using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeRecording.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TargetTimeModelTimes_TargetTimeModelId",
                table: "TargetTimeModelTimes");

            migrationBuilder.DropIndex(
                name: "IX_PersonTargetTimeModel_PersonId",
                table: "PersonTargetTimeModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TargetTimeModelTimes",
                table: "TargetTimeModelTimes",
                columns: new[] { "TargetTimeModelId", "ValidFrom" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonTargetTimeModel",
                table: "PersonTargetTimeModel",
                columns: new[] { "PersonId", "ValidFrom" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TargetTimeModelTimes",
                table: "TargetTimeModelTimes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonTargetTimeModel",
                table: "PersonTargetTimeModel");

            migrationBuilder.CreateIndex(
                name: "IX_TargetTimeModelTimes_TargetTimeModelId",
                table: "TargetTimeModelTimes",
                column: "TargetTimeModelId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonTargetTimeModel_PersonId",
                table: "PersonTargetTimeModel",
                column: "PersonId");
        }
    }
}
