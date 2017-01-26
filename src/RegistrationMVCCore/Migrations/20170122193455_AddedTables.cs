using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RegistrationMVCCore.Migrations
{
    public partial class AddedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NotesID",
                table: "patientAccount",
                newName: "OccID");

            migrationBuilder.CreateTable(
                name: "guardians",
                columns: table => new
                {
                    GuardianID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    PhoneNo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guardians", x => x.GuardianID);
                });

            migrationBuilder.CreateTable(
                name: "notePatient",
                columns: table => new
                {
                    NoteID = table.Column<int>(nullable: false),
                    PPS_No = table.Column<int>(nullable: false),
                    NoteDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notePatient", x => new { x.NoteID, x.PPS_No });
                    table.UniqueConstraint("AK_notePatient_NoteID", x => x.NoteID);
                });

            migrationBuilder.CreateTable(
                name: "notes",
                columns: table => new
                {
                    NoteID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Details = table.Column<string>(nullable: true),
                    NoteTitle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notes", x => x.NoteID);
                });

            migrationBuilder.CreateTable(
                name: "schoolLists",
                columns: table => new
                {
                    SchoolID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SchoolAddress = table.Column<string>(nullable: true),
                    SchoolName = table.Column<string>(nullable: true),
                    SchoolPhone = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schoolLists", x => x.SchoolID);
                });

            migrationBuilder.CreateTable(
                name: "otTasks",
                columns: table => new
                {
                    TaskID = table.Column<int>(nullable: false),
                    PPS_No = table.Column<int>(nullable: false),
                    OccID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_otTasks", x => new { x.TaskID, x.PPS_No, x.OccID });
                    table.UniqueConstraint("AK_otTasks_TaskID", x => x.TaskID);
                    table.UniqueConstraint("AK_otTasks_PPS_No_TaskID", x => new { x.PPS_No, x.TaskID });
                    table.UniqueConstraint("AK_otTasks_OccID_PPS_No_TaskID", x => new { x.OccID, x.PPS_No, x.TaskID });
                });

            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    TaskID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Completed = table.Column<bool>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false),
                    TaskType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tasks", x => x.TaskID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "guardians");

            migrationBuilder.DropTable(
                name: "notePatient");

            migrationBuilder.DropTable(
                name: "notes");

            migrationBuilder.DropTable(
                name: "schoolLists");

            migrationBuilder.DropTable(
                name: "otTasks");

            migrationBuilder.DropTable(
                name: "tasks");

            migrationBuilder.RenameColumn(
                name: "OccID",
                table: "patientAccount",
                newName: "NotesID");
        }
    }
}
