﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using qualityservice.Data;
using System;

namespace qualityservice.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180306181930_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("qualityservice.Model.Analysis", b =>
                {
                    b.Property<int>("analysisId")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("datetime");

                    b.Property<double>("elem_Cu");

                    b.Property<double>("elem_Fe");

                    b.Property<double>("elem_Ni");

                    b.Property<double>("elem_Pb");

                    b.Property<double>("elem_Sn");

                    b.Property<string>("message");

                    b.Property<int>("number");

                    b.Property<int?>("productionOrderQualityId");

                    b.Property<string>("status");

                    b.HasKey("analysisId");

                    b.HasIndex("productionOrderQualityId");

                    b.ToTable("Analyses");
                });

            modelBuilder.Entity("qualityservice.Model.ProductionOrderQuality", b =>
                {
                    b.Property<int>("productionOrderQualityId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("corrida");

                    b.Property<string>("forno");

                    b.Property<string>("posicao");

                    b.Property<int>("productionOrderId");

                    b.Property<string>("productionOrderNumber");

                    b.Property<string>("status");

                    b.HasKey("productionOrderQualityId");

                    b.ToTable("ProductionOrderQualities");
                });

            modelBuilder.Entity("qualityservice.Model.Analysis", b =>
                {
                    b.HasOne("qualityservice.Model.ProductionOrderQuality")
                        .WithMany("Analysis")
                        .HasForeignKey("productionOrderQualityId");
                });
#pragma warning restore 612, 618
        }
    }
}
