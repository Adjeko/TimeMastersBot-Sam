using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TimeMasters.Web.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Environment",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FxProfile = table.Column<string>(nullable: true),
                    IsDebugging = table.Column<bool>(nullable: false),
                    LogID = table.Column<int>(nullable: false),
                    MachineName = table.Column<string>(nullable: true),
                    SessionId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Environment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Environment_Log_LogID",
                        column: x => x.LogID,
                        principalTable: "Log",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Level = table.Column<string>(nullable: true),
                    LogID = table.Column<int>(nullable: false),
                    Logger = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    SequenceID = table.Column<int>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Events_Log_LogID",
                        column: x => x.LogID,
                        principalTable: "Log",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MetroLogVersion",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Build = table.Column<int>(nullable: false),
                    EnvironmentID = table.Column<int>(nullable: false),
                    Major = table.Column<int>(nullable: false),
                    MajorRevision = table.Column<int>(nullable: false),
                    Minor = table.Column<int>(nullable: false),
                    MinorRevision = table.Column<int>(nullable: false),
                    Revision = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetroLogVersion", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MetroLogVersion_Environment_EnvironmentID",
                        column: x => x.EnvironmentID,
                        principalTable: "Environment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exception",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventsID = table.Column<int>(nullable: false),
                    HResult = table.Column<int>(nullable: false),
                    HelpLink = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Source = table.Column<string>(nullable: true),
                    StackTrace = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exception", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Exception_Events_EventsID",
                        column: x => x.EventsID,
                        principalTable: "Events",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExceptionWrapper",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AsString = table.Column<string>(nullable: true),
                    EventsID = table.Column<int>(nullable: false),
                    Hresult = table.Column<int>(nullable: false),
                    TypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionWrapper", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExceptionWrapper_Events_EventsID",
                        column: x => x.EventsID,
                        principalTable: "Events",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Environment_LogID",
                table: "Environment",
                column: "LogID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_LogID",
                table: "Events",
                column: "LogID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exception_EventsID",
                table: "Exception",
                column: "EventsID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionWrapper_EventsID",
                table: "ExceptionWrapper",
                column: "EventsID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MetroLogVersion_EnvironmentID",
                table: "MetroLogVersion",
                column: "EnvironmentID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exception");

            migrationBuilder.DropTable(
                name: "ExceptionWrapper");

            migrationBuilder.DropTable(
                name: "MetroLogVersion");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Environment");

            migrationBuilder.DropTable(
                name: "Log");
        }
    }
}
