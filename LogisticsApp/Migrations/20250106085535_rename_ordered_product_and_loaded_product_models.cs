using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LogisticsApp.Migrations
{
    /// <inheritdoc />
    public partial class rename_ordered_product_and_loaded_product_models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoadedProducts");

            migrationBuilder.DropTable(
                name: "OrderedProductTruck");

            migrationBuilder.DropTable(
                name: "OrderedProducts");

            migrationBuilder.CreateTable(
                name: "ShopFactoryProducts",
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
                    table.PrimaryKey("PK_ShopFactoryProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopFactoryProducts_FactoryProducts_FactoryProductId",
                        column: x => x.FactoryProductId,
                        principalTable: "FactoryProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopFactoryProducts_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopFactoryProductTruck",
                columns: table => new
                {
                    ShopFactoryProductsId = table.Column<int>(type: "integer", nullable: false),
                    TrucksId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopFactoryProductTruck", x => new { x.ShopFactoryProductsId, x.TrucksId });
                    table.ForeignKey(
                        name: "FK_ShopFactoryProductTruck_ShopFactoryProducts_ShopFactoryProd~",
                        column: x => x.ShopFactoryProductsId,
                        principalTable: "ShopFactoryProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopFactoryProductTruck_Trucks_TrucksId",
                        column: x => x.TrucksId,
                        principalTable: "Trucks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TruckShopFactoryProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    TruckId = table.Column<int>(type: "integer", nullable: false),
                    ShopFactoryProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruckShopFactoryProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TruckShopFactoryProducts_ShopFactoryProducts_ShopFactoryPro~",
                        column: x => x.ShopFactoryProductId,
                        principalTable: "ShopFactoryProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TruckShopFactoryProducts_Trucks_TruckId",
                        column: x => x.TruckId,
                        principalTable: "Trucks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopFactoryProducts_FactoryProductId",
                table: "ShopFactoryProducts",
                column: "FactoryProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopFactoryProducts_ShopId",
                table: "ShopFactoryProducts",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopFactoryProductTruck_TrucksId",
                table: "ShopFactoryProductTruck",
                column: "TrucksId");

            migrationBuilder.CreateIndex(
                name: "IX_TruckShopFactoryProducts_ShopFactoryProductId",
                table: "TruckShopFactoryProducts",
                column: "ShopFactoryProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TruckShopFactoryProducts_TruckId",
                table: "TruckShopFactoryProducts",
                column: "TruckId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopFactoryProductTruck");

            migrationBuilder.DropTable(
                name: "TruckShopFactoryProducts");

            migrationBuilder.DropTable(
                name: "ShopFactoryProducts");

            migrationBuilder.CreateTable(
                name: "OrderedProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FactoryProductId = table.Column<int>(type: "integer", nullable: false),
                    ShopId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
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
                    OrderedProductId = table.Column<int>(type: "integer", nullable: false),
                    TruckId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
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
    }
}
