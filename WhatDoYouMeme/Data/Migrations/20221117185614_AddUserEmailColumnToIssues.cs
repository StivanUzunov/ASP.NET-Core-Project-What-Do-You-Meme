using Microsoft.EntityFrameworkCore.Migrations;

namespace WhatDoYouMeme.Data.Migrations
{
    public partial class AddUserEmailColumnToIssues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Issues",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Issues");
        }
    }
}
