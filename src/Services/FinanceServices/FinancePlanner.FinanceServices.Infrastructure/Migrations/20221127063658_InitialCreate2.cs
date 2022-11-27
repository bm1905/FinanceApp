using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancePlanner.FinanceServices.Infrastructure.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AllowanceNumber",
                table: "TaxInformation",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowanceNumber",
                table: "TaxInformation");
        }
    }
}
