using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeRecording.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PersonnelNumber = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TargetTimeModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Model = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetTimeModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkingTime",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Minutes = table.Column<int>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingTime", x => new { x.PersonId, x.Date });
                    table.ForeignKey(
                        name: "FK_WorkingTime_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonTargetTimeModel",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "date", nullable: false),
                    TargetTimeModelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_PersonTargetTimeModel_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonTargetTimeModel_TargetTimeModel_TargetTimeModelId",
                        column: x => x.TargetTimeModelId,
                        principalTable: "TargetTimeModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TargetTimeModelTimes",
                columns: table => new
                {
                    TargetTimeModelId = table.Column<int>(type: "int", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "date", nullable: false),
                    Monday = table.Column<int>(type: "smallint", nullable: false),
                    Tuesday = table.Column<int>(type: "smallint", nullable: false),
                    Wednesday = table.Column<int>(type: "smallint", nullable: false),
                    Thursday = table.Column<int>(type: "smallint", nullable: false),
                    Friday = table.Column<int>(type: "smallint", nullable: false),
                    Saturday = table.Column<int>(type: "smallint", nullable: false),
                    Sunday = table.Column<int>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_TargetTimeModelTimes_TargetTimeModel_TargetTimeModelId",
                        column: x => x.TargetTimeModelId,
                        principalTable: "TargetTimeModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonTargetTimeModel_PersonId",
                table: "PersonTargetTimeModel",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonTargetTimeModel_TargetTimeModelId",
                table: "PersonTargetTimeModel",
                column: "TargetTimeModelId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetTimeModelTimes_TargetTimeModelId",
                table: "TargetTimeModelTimes",
                column: "TargetTimeModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonTargetTimeModel");

            migrationBuilder.DropTable(
                name: "TargetTimeModelTimes");

            migrationBuilder.DropTable(
                name: "WorkingTime");

            migrationBuilder.DropTable(
                name: "TargetTimeModel");

            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
