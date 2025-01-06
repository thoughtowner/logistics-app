using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LogisticsApp.Migrations
{
    /// <inheritdoc />
    public partial class add_ordered_product_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductTruck");

            migrationBuilder.DropTable(
                name: "TruckProducts");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "FactoryProducts",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "FactoryProductShop",
                columns: table => new
                {
                    FactoryProductsId = table.Column<int>(type: "integer", nullable: false),
                    ShopsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactoryProductShop", x => new { x.FactoryProductsId, x.ShopsId });
                    table.ForeignKey(
                        name: "FK_FactoryProductShop_FactoryProducts_FactoryProductsId",
                        column: x => x.FactoryProductsId,
                        principalTable: "FactoryProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FactoryProductShop_Shops_ShopsId",
                        column: x => x.ShopsId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderedProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    ShopId = table.Column<int>(type: "integer", nullable: false),
                    FactoryProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderedProducts_FactoryProducts_FactoryProductId",
                        column: x => x.FactoryProductId,
                        principalTable: "FactoryProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderedProducts_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoadedProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    TruckId = table.Column<int>(type: "integer", nullable: false),
                    OrderedProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadedProducts_OrderedProducts_OrderedProductId",
                        column: x => x.OrderedProductId,
                        principalTable: "OrderedProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoadedProducts_Trucks_TruckId",
                        column: x => x.TruckId,
                        principalTable: "Trucks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderedProductTruck",
                columns: table => new
                {
                    OrderedProductsId = table.Column<int>(type: "integer", nullable: false),
                    TrucksId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderedProductTruck", x => new { x.OrderedProductsId, x.TrucksId });
                    table.ForeignKey(
                        name: "FK_OrderedProductTruck_OrderedProducts_OrderedProductsId",
                        column: x => x.OrderedProductsId,
                        principalTable: "OrderedProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderedProductTruck_Trucks_TrucksId",
                        column: x => x.TrucksId,
                        principalTable: "Trucks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FactoryProductShop_ShopsId",
                table: "FactoryProductShop",
                column: "ShopsId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadedProducts_OrderedProductId",
                table: "LoadedProducts",
                column: "OrderedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadedProducts_TruckId",
                table: "LoadedProducts",
                column: "TruckId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedProducts_FactoryProductId",
                table: "OrderedProducts",
                column: "FactoryProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedProducts_ShopId",
                table: "OrderedProducts",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedProductTruck_TrucksId",
                table: "OrderedProductTruck",
                column: "TrucksId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FactoryProductShop");

            migrationBuilder.DropTable(
                name: "LoadedProducts");

            migrationBuilder.DropTable(
                name: "OrderedProductTruck");

            migrationBuilder.DropTable(
                name: "OrderedProducts");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "FactoryProducts");

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "Products",
                type: "real",
                nullable: false,
                defaultValue: 0f);

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

            migrationBuilder.CreateTable(
                name: "TruckProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    TruckId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruckProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TruckProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TruckProducts_Trucks_TruckId",
                        column: x => x.TruckId,
                        principalTable: "Trucks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductTruck_TrucksId",
                table: "ProductTruck",
                column: "TrucksId");

            migrationBuilder.CreateIndex(
                name: "IX_TruckProducts_ProductId",
                table: "TruckProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TruckProducts_TruckId",
                table: "TruckProducts",
                column: "TruckId");
        }
    }
}
