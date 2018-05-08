using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace qualityservice.Migrations
{
    public partial class ChangeModelProductionOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "productionOrderQualityId",
                table: "MessagesCalculates",
                type: "int4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessagesCalculates_productionOrderQualityId",
                table: "MessagesCalculates",
                column: "productionOrderQualityId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesCalculates_ProductionOrderQualities_productionOrderQualityId",
                table: "MessagesCalculates",
                column: "productionOrderQualityId",
                principalTable: "ProductionOrderQualities",
                principalColumn: "productionOrderQualityId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessagesCalculates_ProductionOrderQualities_productionOrderQualityId",
                table: "MessagesCalculates");

            migrationBuilder.DropIndex(
                name: "IX_MessagesCalculates_productionOrderQualityId",
                table: "MessagesCalculates");

            migrationBuilder.DropColumn(
                name: "productionOrderQualityId",
                table: "MessagesCalculates");
        }
    }
}
