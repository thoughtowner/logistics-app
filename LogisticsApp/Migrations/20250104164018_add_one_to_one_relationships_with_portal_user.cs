using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsApp.Migrations
{
    /// <inheritdoc />
    public partial class add_one_to_one_relationships_with_portal_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PortalUserId",
                table: "Trucks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PortalUserId",
                table: "Shops",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PortalUserId",
                table: "Factories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_PortalUserId",
                table: "Trucks",
                column: "PortalUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shops_PortalUserId",
                table: "Shops",
                column: "PortalUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Factories_PortalUserId",
                table: "Factories",
                column: "PortalUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Factories_AspNetUsers_PortalUserId",
                table: "Factories",
                column: "PortalUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_AspNetUsers_PortalUserId",
                table: "Shops",
                column: "PortalUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trucks_AspNetUsers_PortalUserId",
                table: "Trucks",
                column: "PortalUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Factories_AspNetUsers_PortalUserId",
                table: "Factories");

            migrationBuilder.DropForeignKey(
                name: "FK_Shops_AspNetUsers_PortalUserId",
                table: "Shops");

            migrationBuilder.DropForeignKey(
                name: "FK_Trucks_AspNetUsers_PortalUserId",
                table: "Trucks");

            migrationBuilder.DropIndex(
                name: "IX_Trucks_PortalUserId",
                table: "Trucks");

            migrationBuilder.DropIndex(
                name: "IX_Shops_PortalUserId",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Factories_PortalUserId",
                table: "Factories");

            migrationBuilder.DropColumn(
                name: "PortalUserId",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "PortalUserId",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "PortalUserId",
                table: "Factories");
        }
    }
}
