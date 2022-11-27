using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancePlanner.FinanceServices.Infrastructure.Migrations
{
    public partial class InitialCreate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FederalTaxWithheldAmount",
                table: "IncomeInformation",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MedicareWithheldAmount",
                table: "IncomeInformation",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SocialSecurityWithheldAmount",
                table: "IncomeInformation",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "StateTaxWithheldAmount",
                table: "IncomeInformation",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalTaxesWithheldAmount",
                table: "IncomeInformation",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FederalTaxWithheldAmount",
                table: "IncomeInformation");

            migrationBuilder.DropColumn(
                name: "MedicareWithheldAmount",
                table: "IncomeInformation");

            migrationBuilder.DropColumn(
                name: "SocialSecurityWithheldAmount",
                table: "IncomeInformation");

            migrationBuilder.DropColumn(
                name: "StateTaxWithheldAmount",
                table: "IncomeInformation");

            migrationBuilder.DropColumn(
                name: "TotalTaxesWithheldAmount",
                table: "IncomeInformation");
        }
    }
}
