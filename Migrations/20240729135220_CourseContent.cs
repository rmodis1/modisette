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
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Semester = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => new { x.Code, x.Year, x.Semester });
                });

            migrationBuilder.CreateTable(
                name: "CourseDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseCode = table.Column<string>(type: "TEXT", nullable: false),
                    CourseYear = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseSemester = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Document = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseDocuments_Courses_CourseCode_CourseYear_CourseSemester",
                        columns: x => new { x.CourseCode, x.CourseYear, x.CourseSemester },
                        principalTable: "Courses",
                        principalColumns: new[] { "Code", "Year", "Semester" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseDocuments_CourseCode_CourseYear_CourseSemester",
                table: "CourseDocuments",
                columns: new[] { "CourseCode", "CourseYear", "CourseSemester" });
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
