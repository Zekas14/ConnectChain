using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConnectChain.Migrations
{
    /// <inheritdoc />
    public partial class QuotationUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Quotations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_ProductId",
                table: "Quotations",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotations_Products_ProductId",
                table: "Quotations",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotations_Products_ProductId",
                table: "Quotations");

            migrationBuilder.DropIndex(
                name: "IX_Quotations_ProductId",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Quotations");
        }
    }
}
