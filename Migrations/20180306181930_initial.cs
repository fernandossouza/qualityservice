using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace qualityservice.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductionOrderQualities",
                columns: table => new
                {
                    productionOrderQualityId = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    corrida = table.Column<int>(type: "int4", nullable: false),
                    forno = table.Column<string>(type: "text", nullable: true),
                    posicao = table.Column<string>(type: "text", nullable: true),
                    productionOrderId = table.Column<int>(type: "int4", nullable: false),
                    productionOrderNumber = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionOrderQualities", x => x.productionOrderQualityId);
                });

            migrationBuilder.CreateTable(
                name: "Analyses",
                columns: table => new
                {
                    analysisId = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    datetime = table.Column<long>(type: "int8", nullable: false),
                    elem_Cu = table.Column<double>(type: "float8", nullable: false),
                    elem_Fe = table.Column<double>(type: "float8", nullable: false),
                    elem_Ni = table.Column<double>(type: "float8", nullable: false),
                    elem_Pb = table.Column<double>(type: "float8", nullable: false),
                    elem_Sn = table.Column<double>(type: "float8", nullable: false),
                    message = table.Column<string>(type: "text", nullable: true),
                    number = table.Column<int>(type: "int4", nullable: false),
                    productionOrderQualityId = table.Column<int>(type: "int4", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analyses", x => x.analysisId);
                    table.ForeignKey(
                        name: "FK_Analyses_ProductionOrderQualities_productionOrderQualityId",
                        column: x => x.productionOrderQualityId,
                        principalTable: "ProductionOrderQualities",
                        principalColumn: "productionOrderQualityId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_productionOrderQualityId",
                table: "Analyses",
                column: "productionOrderQualityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Analyses");

            migrationBuilder.DropTable(
                name: "ProductionOrderQualities");
        }
    }
}
