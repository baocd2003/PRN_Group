﻿// <auto-generated />
using System;
using DataAccessLayer.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccessLayer.Migrations
{
    [DbContext(typeof(applicationDbContext))]
    partial class applicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.27")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BatchQuotation", b =>
                {
                    b.Property<Guid>("BatchsBatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("QuotationsQuotationId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BatchsBatchId", "QuotationsQuotationId");

                    b.HasIndex("QuotationsQuotationId");

                    b.ToTable("BatchQuotation");
                });

            modelBuilder.Entity("BussinessObject.Entity.Batch", b =>
                {
                    b.Property<Guid>("BatchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AdminId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ImportDate")
                        .HasColumnType("datetime2");

                    b.HasKey("BatchId");

                    b.ToTable("Batches");
                });

            modelBuilder.Entity("BussinessObject.Entity.BatchDetail", b =>
                {
                    b.Property<Guid>("BatchDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MaterialId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.HasKey("BatchDetailId");

                    b.HasIndex("BatchId");

                    b.HasIndex("MaterialId");

                    b.ToTable("BatchDetails");
                });

            modelBuilder.Entity("BussinessObject.Entity.Material", b =>
                {
                    b.Property<Guid>("MaterialId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("MediumPrice")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UnitType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaterialId");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("BussinessObject.Entity.Project", b =>
                {
                    b.Property<Guid>("ProjectID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("AreaPerFloor")
                        .HasColumnType("real");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("LaborSalaryPerMonth")
                        .HasColumnType("real");

                    b.Property<int>("MonthDuration")
                        .HasColumnType("int");

                    b.Property<int>("NumOfFloors")
                        .HasColumnType("int");

                    b.Property<int>("NumOfLabors")
                        .HasColumnType("int");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("Status")
                        .HasColumnType("tinyint");

                    b.Property<float>("TotalPrice")
                        .HasColumnType("real");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ProjectID");

                    b.HasIndex("UserId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("BussinessObject.Entity.ProjectMaterial", b =>
                {
                    b.Property<Guid>("ProjectMaterialId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MaterialId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("ProjectMaterialId");

                    b.HasIndex("MaterialId");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectMaterials");
                });

            modelBuilder.Entity("BussinessObject.Entity.Quotation", b =>
                {
                    b.Property<Guid>("QuotationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("CompletePrice")
                        .HasColumnType("float");

                    b.Property<Guid?>("CustomerUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("EstimatePrice")
                        .HasColumnType("float");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("StaffUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("QuotationId");

                    b.HasIndex("CustomerUserId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("StaffUserId");

                    b.ToTable("Quotations");
                });

            modelBuilder.Entity("BussinessObject.Entity.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("BussinessObject.Entity.Customer", b =>
                {
                    b.HasBaseType("BussinessObject.Entity.User");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasDiscriminator().HasValue("Customer");
                });

            modelBuilder.Entity("BussinessObject.Entity.Staff", b =>
                {
                    b.HasBaseType("BussinessObject.Entity.User");

                    b.Property<Guid>("StaffId")
                        .HasColumnType("uniqueidentifier");

                    b.HasDiscriminator().HasValue("Staff");
                });

            modelBuilder.Entity("BatchQuotation", b =>
                {
                    b.HasOne("BussinessObject.Entity.Batch", null)
                        .WithMany()
                        .HasForeignKey("BatchsBatchId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BussinessObject.Entity.Quotation", null)
                        .WithMany()
                        .HasForeignKey("QuotationsQuotationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("BussinessObject.Entity.BatchDetail", b =>
                {
                    b.HasOne("BussinessObject.Entity.Batch", "Batch")
                        .WithMany("BatchDetails")
                        .HasForeignKey("BatchId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BussinessObject.Entity.Material", "Materials")
                        .WithMany()
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Batch");

                    b.Navigation("Materials");
                });

            modelBuilder.Entity("BussinessObject.Entity.Project", b =>
                {
                    b.HasOne("BussinessObject.Entity.User", null)
                        .WithMany("Projects")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BussinessObject.Entity.ProjectMaterial", b =>
                {
                    b.HasOne("BussinessObject.Entity.Material", "Materials")
                        .WithMany("ProjectMaterials")
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BussinessObject.Entity.Project", "Projects")
                        .WithMany("ProjectMaterials")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Materials");

                    b.Navigation("Projects");
                });

            modelBuilder.Entity("BussinessObject.Entity.Quotation", b =>
                {
                    b.HasOne("BussinessObject.Entity.Customer", null)
                        .WithMany("Quotations")
                        .HasForeignKey("CustomerUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BussinessObject.Entity.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BussinessObject.Entity.Staff", null)
                        .WithMany("Quotations")
                        .HasForeignKey("StaffUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Project");
                });

            modelBuilder.Entity("BussinessObject.Entity.Batch", b =>
                {
                    b.Navigation("BatchDetails");
                });

            modelBuilder.Entity("BussinessObject.Entity.Material", b =>
                {
                    b.Navigation("ProjectMaterials");
                });

            modelBuilder.Entity("BussinessObject.Entity.Project", b =>
                {
                    b.Navigation("ProjectMaterials");
                });

            modelBuilder.Entity("BussinessObject.Entity.User", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("BussinessObject.Entity.Customer", b =>
                {
                    b.Navigation("Quotations");
                });

            modelBuilder.Entity("BussinessObject.Entity.Staff", b =>
                {
                    b.Navigation("Quotations");
                });
#pragma warning restore 612, 618
        }
    }
}
