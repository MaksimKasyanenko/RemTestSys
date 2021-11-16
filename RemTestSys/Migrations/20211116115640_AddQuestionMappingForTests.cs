using Microsoft.EntityFrameworkCore.Migrations;

namespace RemTestSys.Migrations
{
    public partial class AddQuestionMappingForTests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionsCount",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "ScoresPerRightAnswer",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "RightAnswersCount",
                table: "Sessions");

            migrationBuilder.AddColumn<double>(
                name: "Scores",
                table: "Sessions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "Mark",
                table: "ResultsOfTesting",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "Cast",
                table: "Questions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "MapParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionCount = table.Column<int>(type: "int", nullable: false),
                    QuestionCast = table.Column<double>(type: "float", nullable: false),
                    TestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapParts_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapParts_TestId",
                table: "MapParts",
                column: "TestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MapParts");

            migrationBuilder.DropColumn(
                name: "Scores",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "Cast",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "QuestionsCount",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ScoresPerRightAnswer",
                table: "Tests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "RightAnswersCount",
                table: "Sessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Mark",
                table: "ResultsOfTesting",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
