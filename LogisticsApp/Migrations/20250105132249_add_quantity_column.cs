using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsApp.Migrations
{
    /// <inheritdoc />
    public partial class add_quantity_column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "TruckProducts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ShopProducts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "FactoryProducts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "TruckProducts");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ShopProducts");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "FactoryProducts");
        }
    }
}
