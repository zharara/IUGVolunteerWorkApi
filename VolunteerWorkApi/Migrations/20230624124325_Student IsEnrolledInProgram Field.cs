using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolunteerWorkApi.Migrations
{
    /// <inheritdoc />
    public partial class StudentIsEnrolledInProgramField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterestStudent");

            migrationBuilder.DropTable(
                name: "Interests");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnrolledInProgram",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnrolledInProgram",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Interests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VolunteerOpportunityId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interests_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Interests_VolunteerOpportunities_VolunteerOpportunityId",
                        column: x => x.VolunteerOpportunityId,
                        principalTable: "VolunteerOpportunities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InterestStudent",
                columns: table => new
                {
                    InterestsId = table.Column<long>(type: "bigint", nullable: false),
                    StudentsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestStudent", x => new { x.InterestsId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_InterestStudent_AspNetUsers_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterestStudent_Interests_InterestsId",
                        column: x => x.InterestsId,
                        principalTable: "Interests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Interests_CategoryId",
                table: "Interests",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Interests_VolunteerOpportunityId",
                table: "Interests",
                column: "VolunteerOpportunityId");

            migrationBuilder.CreateIndex(
                name: "IX_InterestStudent_StudentsId",
                table: "InterestStudent",
                column: "StudentsId");
        }
    }
}
