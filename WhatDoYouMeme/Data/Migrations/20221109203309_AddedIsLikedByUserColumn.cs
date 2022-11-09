using Microsoft.EntityFrameworkCore.Migrations;

namespace WhatDoYouMeme.Data.Migrations
{
    public partial class AddedIsLikedByUserColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLikedByUser",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLikedByUser",
                table: "Posts");
        }
    }
}
