using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace qualityservice.Migrations
{
    public partial class CreateFieldRecipeCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "recipeCode",
                table: "ProductionOrderQualities",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "recipeCode",
                table: "ProductionOrderQualities");
        }
    }
}
