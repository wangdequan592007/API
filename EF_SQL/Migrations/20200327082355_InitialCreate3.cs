using Microsoft.EntityFrameworkCore.Migrations;

namespace EF_SQL.Migrations
{
    public partial class InitialCreate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DT_VALUE",
                table: "DA_ELSEGROSSs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DT_VALUE",
                table: "DA_ELSEGROSSs");
        }
    }
}
