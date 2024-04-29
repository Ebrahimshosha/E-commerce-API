using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalabatRebosatiory.Data.Migrations
{
    public partial class setondeletecascedefororder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "product_ProductName",
                table: "OrderItems",
                newName: "Product_ProductName");

            migrationBuilder.RenameColumn(
                name: "product_ProductId",
                table: "OrderItems",
                newName: "Product_ProductId");

            migrationBuilder.RenameColumn(
                name: "product_PictureUrl",
                table: "OrderItems",
                newName: "Product_PictureUrl");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "Product_ProductName",
                table: "OrderItems",
                newName: "product_ProductName");

            migrationBuilder.RenameColumn(
                name: "Product_ProductId",
                table: "OrderItems",
                newName: "product_ProductId");

            migrationBuilder.RenameColumn(
                name: "Product_PictureUrl",
                table: "OrderItems",
                newName: "product_PictureUrl");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
