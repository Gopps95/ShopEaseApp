using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopEaseApp.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderDetails_OrderDetailsID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderDetailsID",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "OrderDetailsID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderDetailsID",
                table: "OrderDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderDetailsID",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailsID",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails",
                column: "OrderDetailsID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderDetailsID",
                table: "Orders",
                column: "OrderDetailsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderDetails_OrderDetailsID",
                table: "Orders",
                column: "OrderDetailsID",
                principalTable: "OrderDetails",
                principalColumn: "OrderDetailsID");
        }
    }
}
