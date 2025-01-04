using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsApp.Migrations
{
    /// <inheritdoc />
    public partial class add_skip_navigations_to_all_models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FactoryId",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TruckId",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_FactoryId",
                table: "Products",
                column: "FactoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ShopId",
                table: "Products",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_TruckId",
                table: "Products",
                column: "TruckId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Factories_FactoryId",
                table: "Products",
                column: "FactoryId",
                principalTable: "Factories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Shops_ShopId",
                table: "Products",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Trucks_TruckId",
                table: "Products",
                column: "TruckId",
                principalTable: "Trucks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Factories_FactoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Shops_ShopId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Trucks_TruckId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FactoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ShopId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_TruckId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FactoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TruckId",
                table: "Products");
        }
    }
}
