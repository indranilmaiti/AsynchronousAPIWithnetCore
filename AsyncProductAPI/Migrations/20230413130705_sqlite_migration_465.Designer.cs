﻿// <auto-generated />
using AsyncProductAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AsyncProductAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230413130705_sqlite_migration_465")]
    partial class sqlite_migration_465
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("AsyncProductAPI.Models.HeavyProcessingRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EstimatedCompetionTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("RequestBody")
                        .HasColumnType("TEXT");

                    b.Property<string>("RequestId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RequestStatus")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("HeavyProcessingRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
