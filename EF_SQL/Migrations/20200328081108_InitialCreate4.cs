using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EF_SQL.Migrations
{
    public partial class InitialCreate4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CRT_DATE",
                table: "DA_ELSEGROSSs",
                nullable: true,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DA_PAYLOSSs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DT_CLIENTNAME = table.Column<string>(nullable: true),
                    DT_MODEL = table.Column<int>(nullable: true),
                    DT_NAME = table.Column<string>(nullable: true),
                    DT_BOM = table.Column<string>(nullable: true),
                    DT_PRICE = table.Column<string>(nullable: true),
                    DT_LOSS = table.Column<string>(nullable: true),
                    DT_DATE = table.Column<DateTime>(nullable: true),
                    CRT_DATE = table.Column<DateTime>(nullable: true, defaultValueSql: "GETDATE()"),
                    CRT_USER = table.Column<string>(nullable: true),
                    DT_REMARKS = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DA_PAYLOSSs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DA_PAYMONTHs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DT_CLIENTNAME = table.Column<string>(nullable: true),
                    DT_MODEL = table.Column<int>(nullable: true),
                    DT_NAME = table.Column<string>(nullable: true),
                    DT_EXPEND = table.Column<string>(nullable: true),
                    DT_DATE = table.Column<DateTime>(nullable: true),
                    CRT_DATE = table.Column<DateTime>(nullable: true, defaultValueSql: "GETDATE()"),
                    CRT_USER = table.Column<string>(nullable: true),
                    DT_REMARKS = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DA_PAYMONTHs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DA_PAYLOSSs");

            migrationBuilder.DropTable(
                name: "DA_PAYMONTHs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CRT_DATE",
                table: "DA_ELSEGROSSs",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "GETDATE()");
        }
    }
}
