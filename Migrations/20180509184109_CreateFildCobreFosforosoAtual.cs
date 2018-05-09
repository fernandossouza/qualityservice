using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace qualityservice.Migrations
{
    public partial class CreateFildCobreFosforosoAtual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CobreFosforosoAtual",
                table: "ProductionOrderQualities",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CobreFosforosoAtual",
                table: "ProductionOrderQualities");
        }
    }
}
