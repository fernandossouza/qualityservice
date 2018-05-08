using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace qualityservice.Migrations
{
    public partial class Initial : Migration
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
                    cobreFosforoso = table.Column<string>(type: "text", nullable: true),
                    datetime = table.Column<long>(type: "int8", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "AnalysisComps",
                columns: table => new
                {
                    analysisCompId = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    analysisId = table.Column<int>(type: "int4", nullable: true),
                    productId = table.Column<int>(type: "int4", nullable: false),
                    productName = table.Column<string>(type: "text", nullable: true),
                    value = table.Column<double>(type: "float8", nullable: false),
                    valueKg = table.Column<double>(type: "float8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisComps", x => x.analysisCompId);
                    table.ForeignKey(
                        name: "FK_AnalysisComps_Analyses_analysisId",
                        column: x => x.analysisId,
                        principalTable: "Analyses",
                        principalColumn: "analysisId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MessagesCalculates",
                columns: table => new
                {
                    messageId = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    analysisId = table.Column<int>(type: "int4", nullable: true),
                    message = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessagesCalculates", x => x.messageId);
                    table.ForeignKey(
                        name: "FK_MessagesCalculates_Analyses_analysisId",
                        column: x => x.analysisId,
                        principalTable: "Analyses",
                        principalColumn: "analysisId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_productionOrderQualityId",
                table: "Analyses",
                column: "productionOrderQualityId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisComps_analysisId",
                table: "AnalysisComps",
                column: "analysisId");

            migrationBuilder.CreateIndex(
                name: "IX_MessagesCalculates_analysisId",
                table: "MessagesCalculates",
                column: "analysisId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalysisComps");

            migrationBuilder.DropTable(
                name: "MessagesCalculates");

            migrationBuilder.DropTable(
                name: "Analyses");

            migrationBuilder.DropTable(
                name: "ProductionOrderQualities");
        }
    }
}
