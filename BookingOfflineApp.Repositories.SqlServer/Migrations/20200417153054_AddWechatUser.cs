using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookingOfflineApp.Repositories.SqlServer.Migrations
{
    public partial class AddWechatUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WechatUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    OpenId = table.Column<string>(nullable: true),
                    UnionId = table.Column<string>(nullable: true),
                    NickName = table.Column<string>(nullable: true),
                    AvatarUrl = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WechatUsers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WechatUsers");
        }
    }
}
