using Microsoft.EntityFrameworkCore.Migrations;

namespace BookingOfflineApp.Repositories.SqlServer.Migrations
{
    public partial class RemoveProductId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrderItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "OrderItems",
                type: "int",
                nullable: true);
        }
    }
}
