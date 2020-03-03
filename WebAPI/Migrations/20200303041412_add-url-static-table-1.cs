using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class addurlstatictable1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UrlStatistic",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    ShortUrlId = table.Column<string>(nullable: true),
                    IpAddress = table.Column<string>(nullable: true),
                    RefererUrl = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlStatistic", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlStatistic");
        }
    }
}
