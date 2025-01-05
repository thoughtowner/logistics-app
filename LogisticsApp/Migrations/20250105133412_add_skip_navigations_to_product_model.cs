using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsApp.Migrations
{
    /// <inheritdoc />
    public partial class add_skip_navigations_to_product_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "FactoryProduct",
                columns: table => new
                {
                    FactoriesId = table.Column<int>(type: "integer", nullable: false),
                    ProductsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactoryProduct", x => new { x.FactoriesId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_FactoryProduct_Factories_FactoriesId",
                        column: x => x.FactoriesId,
                        principalTable: "Factories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FactoryProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductShop",
                columns: table => new
                {
                    ProductsId = table.Column<int>(type: "integer", nullable: false),
                    ShopsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductShop", x => new { x.ProductsId, x.ShopsId });
                    table.ForeignKey(
                        name: "FK_ProductShop_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductShop_Shops_ShopsId",
                        column: x => x.ShopsId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTruck",
                columns: table => new
                {
                    ProductsId = table.Column<int>(type: "integer", nullable: false),
                    TrucksId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTruck", x => new { x.ProductsId, x.TrucksId });
                    table.ForeignKey(
                        name: "FK_ProductTruck_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTruck_Trucks_TrucksId",
                        column: x => x.TrucksId,
                        principalTable: "Trucks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FactoryProduct_ProductsId",
                table: "FactoryProduct",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductShop_ShopsId",
                table: "ProductShop",
                column: "ShopsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTruck_TrucksId",
                table: "ProductTruck",
                column: "TrucksId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FactoryProduct");

            migrationBuilder.DropTable(
                name: "ProductShop");

            migrationBuilder.DropTable(
                name: "ProductTruck");

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
    }
}
