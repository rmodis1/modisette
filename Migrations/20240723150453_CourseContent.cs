using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace modisette.Migrations
{
    /// <inheritdoc />
    public partial class CourseContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Contact",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Semester = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => new { x.Number, x.Year, x.Semester });
                });

            migrationBuilder.CreateTable(
                name: "CourseDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseYear = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseSemester = table.Column<int>(type: "INTEGER", nullable: false),
                    Document = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseDocuments_Courses_CourseNumber_CourseYear_CourseSemester",
                        columns: x => new { x.CourseNumber, x.CourseYear, x.CourseSemester },
                        principalTable: "Courses",
                        principalColumns: new[] { "Number", "Year", "Semester" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseDocuments_CourseNumber_CourseYear_CourseSemester",
                table: "CourseDocuments",
                columns: new[] { "CourseNumber", "CourseYear", "CourseSemester" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseDocuments");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Contact",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
