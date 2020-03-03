using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class addtitletourltable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Ownerid",
                table: "Url",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Url",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Url_Ownerid",
                table: "Url",
                column: "Ownerid");

            migrationBuilder.AddForeignKey(
                name: "FK_Url_User_Ownerid",
                table: "Url",
                column: "Ownerid",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Url_User_Ownerid",
                table: "Url");

            migrationBuilder.DropIndex(
                name: "IX_Url_Ownerid",
                table: "Url");

            migrationBuilder.DropColumn(
                name: "Ownerid",
                table: "Url");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Url");
        }
    }
}
