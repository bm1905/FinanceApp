﻿// <auto-generated />
using FinancePlanner.FinanceServices.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FinancePlanner.FinanceServices.Infrastructure.Migrations
{
    [DbContext(typeof(FinanceDbContext))]
    [Migration("20221129231517_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FinancePlanner.FinanceServices.Domain.Entities.BiWeeklyHoursAndRate", b =>
                {
                    b.Property<int>("BiWeeklyHoursAndRateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BiWeeklyHoursAndRateId"), 1L, 1);

                    b.Property<decimal>("HourlyRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Week1TimeOffHours")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Week1TotalHours")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Week2TimeOffHours")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Week2TotalHours")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("BiWeeklyHoursAndRateId");

                    b.ToTable("BiWeeklyHoursAndRates");
                });

            modelBuilder.Entity("FinancePlanner.FinanceServices.Domain.Entities.IncomeInformation", b =>
                {
                    b.Property<int>("IncomeInformationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IncomeInformationId"), 1L, 1);

                    b.Property<string>("EmployeeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("GrossPay")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("NetPay")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("PayInformationId")
                        .HasColumnType("int");

                    b.Property<decimal>("PayRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TaxWithheldInformationId")
                        .HasColumnType("int");

                    b.Property<int>("TaxableWageInformationId")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalHours")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalPostTaxDeductions")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalPreTaxDeductions")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IncomeInformationId");

                    b.HasIndex("TaxWithheldInformationId");

                    b.HasIndex("TaxableWageInformationId");

                    b.ToTable("IncomeInformation");
                });

            modelBuilder.Entity("FinancePlanner.FinanceServices.Domain.Entities.PayInformation", b =>
                {
                    b.Property<int>("PayInformationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PayInformationId"), 1L, 1);

                    b.Property<int>("BiWeeklyHoursAndRateId")
                        .HasColumnType("int");

                    b.Property<string>("EmployeeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PostTaxDeductionId")
                        .HasColumnType("int");

                    b.Property<int>("PreTaxDeductionId")
                        .HasColumnType("int");

                    b.Property<int>("TaxInformationId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PayInformationId");

                    b.HasIndex("BiWeeklyHoursAndRateId");

                    b.HasIndex("PostTaxDeductionId");

                    b.HasIndex("PreTaxDeductionId");

                    b.HasIndex("TaxInformationId");

                    b.ToTable("PayInformation");
                });

            modelBuilder.Entity("FinancePlanner.FinanceServices.Domain.Entities.PostTaxDeduction", b =>
                {
                    b.Property<int>("PostTaxDeductionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PostTaxDeductionId"), 1L, 1);

                    b.Property<decimal>("AccidentInsuranceAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("EmployeeStockPlanAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("LifeInsuranceAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MiscellaneousAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Roth401KPercentage")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("PostTaxDeductionId");

                    b.ToTable("PostTaxDeductions");
                });

            modelBuilder.Entity("FinancePlanner.FinanceServices.Domain.Entities.PreTaxDeduction", b =>
                {
                    b.Property<int>("PreTaxDeductionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PreTaxDeductionId"), 1L, 1);

                    b.Property<decimal>("Dental")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("HealthSavingAccountAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Medical")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MiscellaneousAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Traditional401KPercentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Vision")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("PreTaxDeductionId");

                    b.ToTable("PreTaxDeductions");
                });

            modelBuilder.Entity("FinancePlanner.FinanceServices.Domain.Entities.TaxableWageInformation", b =>
                {
                    b.Property<int>("TaxableWageInformationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TaxableWageInformationId"), 1L, 1);

                    b.Property<decimal>("SocialAndMedicareTaxableWages")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("StateAndFederalTaxableWages")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("TaxableWageInformationId");

                    b.ToTable("TaxableWageInformation");
                });

            modelBuilder.Entity("FinancePlanner.FinanceServices.Domain.Entities.TaxInformation", b =>
                {
                    b.Property<int>("TaxInformationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TaxInformationId"), 1L, 1);

                    b.Property<int>("AllowanceNumber")
                        .HasColumnType("int");

                    b.Property<decimal>("ClaimDependentsAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("DeductionsAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ExtraWithholdingAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsMultipleJobsChecked")
                        .HasColumnType("bit");

                    b.Property<decimal>("OtherIncomeAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("PayPeriodNumber")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TaxFilingStatus")
                        .HasColumnType("int");

                    b.Property<int>("W4Type")
                        .HasColumnType("int");

                    b.HasKey("TaxInformationId");

                    b.ToTable("TaxInformation");
                });

            modelBuilder.Entity("FinancePlanner.FinanceServices.Domain.Entities.TaxWithheldInformation", b =>
                {
                    b.Property<int>("TaxWithheldInformationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TaxWithheldInformationId"), 1L, 1);

                    b.Property<decimal>("FederalTaxWithheldAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MedicareWithheldAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("SocialSecurityWithheldAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("StateTaxWithheldAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalTaxesWithheldAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("TaxWithheldInformationId");

                    b.ToTable("TaxWithheldInformation");
                });

            modelBuilder.Entity("FinancePlanner.FinanceServices.Domain.Entities.IncomeInformation", b =>
                {
                    b.HasOne("FinancePlanner.FinanceServices.Domain.Entities.TaxWithheldInformation", "TaxWithheldInformation")
                        .WithMany()
                        .HasForeignKey("TaxWithheldInformationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FinancePlanner.FinanceServices.Domain.Entities.TaxableWageInformation", "TaxableWageInformation")
                        .WithMany()
                        .HasForeignKey("TaxableWageInformationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TaxWithheldInformation");

                    b.Navigation("TaxableWageInformation");
                });

            modelBuilder.Entity("FinancePlanner.FinanceServices.Domain.Entities.PayInformation", b =>
                {
                    b.HasOne("FinancePlanner.FinanceServices.Domain.Entities.BiWeeklyHoursAndRate", "BiWeeklyHoursAndRate")
                        .WithMany()
                        .HasForeignKey("BiWeeklyHoursAndRateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FinancePlanner.FinanceServices.Domain.Entities.PostTaxDeduction", "PostTaxDeduction")
                        .WithMany()
                        .HasForeignKey("PostTaxDeductionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FinancePlanner.FinanceServices.Domain.Entities.PreTaxDeduction", "PreTaxDeduction")
                        .WithMany()
                        .HasForeignKey("PreTaxDeductionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FinancePlanner.FinanceServices.Domain.Entities.TaxInformation", "TaxInformation")
                        .WithMany()
                        .HasForeignKey("TaxInformationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BiWeeklyHoursAndRate");

                    b.Navigation("PostTaxDeduction");

                    b.Navigation("PreTaxDeduction");

                    b.Navigation("TaxInformation");
                });
#pragma warning restore 612, 618
        }
    }
}
