using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EF_SQL.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DA_BOMs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BOM = table.Column<string>(nullable: true),
                    PRICE = table.Column<string>(nullable: true),
                    CRT_DATE = table.Column<DateTime>(nullable: true, defaultValueSql: "GETDATE()"),
                    CRT_USER = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DA_BOMs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DA_ELSEGROSSs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DT_CLIENTNAME = table.Column<string>(nullable: true),
                    DT_MODEL = table.Column<int>(nullable: true),
                    DT_NAME = table.Column<string>(nullable: true),
                    DT_DATE = table.Column<DateTime>(nullable: true),
                    CRT_DATE = table.Column<DateTime>(nullable: true),
                    CRT_USER = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DA_ELSEGROSSs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DA_GROSSs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DT_CLIENTNAME = table.Column<string>(nullable: true),
                    DT_MODEL = table.Column<int>(nullable: true),
                    DT_NAME = table.Column<string>(nullable: true),
                    DT_MO = table.Column<string>(nullable: true),
                    DT_BOM = table.Column<string>(nullable: true),
                    DT_PRICE = table.Column<string>(nullable: true),
                    DT_STORENB = table.Column<string>(nullable: true),
                    DT_MANAGER = table.Column<string>(nullable: true),
                    DT_ON = table.Column<string>(nullable: true),
                    DT_IN = table.Column<string>(nullable: true),
                    DT_OUT = table.Column<string>(nullable: true),
                    DT_OFFWORK = table.Column<string>(nullable: true),
                    DT_ONWORK = table.Column<string>(nullable: true),
                    DT_PERSONHOUR = table.Column<string>(nullable: true),
                    DT_DATE = table.Column<DateTime>(nullable: true),
                    CRT_DATE = table.Column<DateTime>(nullable: true, defaultValueSql: "GETDATE()"),
                    CRT_USER = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DA_GROSSs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DA_PAYPERSONs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DT_CLIENTNAME = table.Column<string>(nullable: true),
                    DT_MODEL = table.Column<int>(nullable: true),
                    DT_NAME = table.Column<string>(nullable: true),
                    DT_INDIRECTWORKTIME = table.Column<string>(nullable: true),
                    DT_INDIRECTWAGE = table.Column<string>(nullable: true),
                    DT_INDIRECTMOUTH = table.Column<string>(nullable: true),
                    DT_DIRECTHOUR = table.Column<string>(nullable: true),
                    DT_WAGE = table.Column<string>(nullable: true),
                    DT_DATE = table.Column<DateTime>(nullable: true),
                    CRT_DATE = table.Column<DateTime>(nullable: true, defaultValueSql: "GETDATE()"),
                    CRT_USER = table.Column<string>(nullable: true),
                    DT_REMARKS = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DA_PAYPERSONs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DA_BOMs");

            migrationBuilder.DropTable(
                name: "DA_ELSEGROSSs");

            migrationBuilder.DropTable(
                name: "DA_GROSSs");

            migrationBuilder.DropTable(
                name: "DA_PAYPERSONs");
        }
    }
}
