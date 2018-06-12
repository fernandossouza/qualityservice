using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace qualityservice.Migrations
{
    public partial class UsernameColumnAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "username",
                table: "ProductionOrderQualities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "username",
                table: "Analyses",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "username",
                table: "ProductionOrderQualities");

            migrationBuilder.DropColumn(
                name: "username",
                table: "Analyses");
        }
    }
}
