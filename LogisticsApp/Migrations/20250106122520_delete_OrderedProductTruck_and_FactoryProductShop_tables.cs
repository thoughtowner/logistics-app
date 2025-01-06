using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsApp.Migrations
{
    /// <inheritdoc />
    public partial class delete_OrderedProductTruck_and_FactoryProductShop_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FactoryProductShop");

            migrationBuilder.DropTable(
                name: "OrderedProductTruck");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "IX_OrderedProductTruck_TrucksId",
                table: "OrderedProductTruck",
                column: "TrucksId");
        }
    }
}
