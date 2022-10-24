using Microsoft.EntityFrameworkCore.Migrations;

namespace WhatDoYouMeme.Data.Migrations
{
    public partial class AddedMemerLogic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemerId",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MemerId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Memers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Memers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_MemerId",
                table: "Posts",
                column: "MemerId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MemerId",
                table: "Comments",
                column: "MemerId");

            migrationBuilder.CreateIndex(
                name: "IX_Memers_UserId",
                table: "Memers",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Memers_MemerId",
                table: "Comments",
                column: "MemerId",
                principalTable: "Memers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Memers_MemerId",
                table: "Posts",
                column: "MemerId",
                principalTable: "Memers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Memers_MemerId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Memers_MemerId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "Memers");

            migrationBuilder.DropIndex(
                name: "IX_Posts_MemerId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Comments_MemerId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "MemerId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "MemerId",
                table: "Comments");
        }
    }
}
