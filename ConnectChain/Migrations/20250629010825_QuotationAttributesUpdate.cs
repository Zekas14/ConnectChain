using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConnectChain.Migrations
{
    /// <inheritdoc />
    public partial class QuotationAttributesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuotedPrice",
                table: "Quotations",
                newName: "UnitPrice");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Quotations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Quotations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Quotations");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "Quotations",
                newName: "QuotedPrice");
        }
    }
}
