using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConnectChain.Migrations
{
    /// <inheritdoc />
    public partial class updateQuotationAttributes3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Quotations_CategoryId",
                table: "Quotations",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
     name: "FK_Quotations_Categories_CategoryId",
     table: "Quotations",
     column: "CategoryId",
     principalTable: "Categories",
     principalColumn: "ID",
     onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotations_Categories_CategoryId",
                table: "Quotations");

            migrationBuilder.DropIndex(
                name: "IX_Quotations_CategoryId",
                table: "Quotations");
        }
    }
}
