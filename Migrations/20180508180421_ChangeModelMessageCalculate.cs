using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace qualityservice.Migrations
{
    public partial class ChangeModelMessageCalculate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "message",
                table: "MessagesCalculates");

            migrationBuilder.AddColumn<string>(
                name: "key",
                table: "MessagesCalculates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "value",
                table: "MessagesCalculates",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "key",
                table: "MessagesCalculates");

            migrationBuilder.DropColumn(
                name: "value",
                table: "MessagesCalculates");

            migrationBuilder.AddColumn<string>(
                name: "message",
                table: "MessagesCalculates",
                nullable: true);
        }
    }
}
