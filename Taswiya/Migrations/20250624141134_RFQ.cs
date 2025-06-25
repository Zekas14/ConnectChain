using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConnectChain.Migrations
{
    /// <inheritdoc />
    public partial class RFQ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RFQ_Customer_CustomerId",
                table: "RFQ");

            migrationBuilder.DropForeignKey(
                name: "FK_RfqAttachment_RFQ_RfqId",
                table: "RfqAttachment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RfqAttachment",
                table: "RfqAttachment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RFQ",
                table: "RFQ");

            migrationBuilder.RenameTable(
                name: "RfqAttachment",
                newName: "RfqAttachments");

            migrationBuilder.RenameTable(
                name: "RFQ",
                newName: "RFQs");

            migrationBuilder.RenameIndex(
                name: "IX_RfqAttachment_RfqId",
                table: "RfqAttachments",
                newName: "IX_RfqAttachments_RfqId");

            migrationBuilder.RenameIndex(
                name: "IX_RFQ_CustomerId",
                table: "RFQs",
                newName: "IX_RFQs_CustomerId");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "RFQs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "RFQs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "RFQs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "RFQs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RfqAttachments",
                table: "RfqAttachments",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RFQs",
                table: "RFQs",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "Quotations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RfqId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuotedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryTimeInDays = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Quotations_RFQs_RfqId",
                        column: x => x.RfqId,
                        principalTable: "RFQs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Quotations_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RfqSupplierAssignments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RfqId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RfqSupplierAssignments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RfqSupplierAssignments_RFQs_RfqId",
                        column: x => x.RfqId,
                        principalTable: "RFQs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RfqSupplierAssignments_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RFQs_ProductId",
                table: "RFQs",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_RfqId",
                table: "Quotations",
                column: "RfqId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_SupplierId",
                table: "Quotations",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_RfqSupplierAssignments_RfqId",
                table: "RfqSupplierAssignments",
                column: "RfqId");

            migrationBuilder.CreateIndex(
                name: "IX_RfqSupplierAssignments_SupplierId",
                table: "RfqSupplierAssignments",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_RfqAttachments_RFQs_RfqId",
                table: "RfqAttachments",
                column: "RfqId",
                principalTable: "RFQs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RFQs_Customer_CustomerId",
                table: "RFQs",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RFQs_Products_ProductId",
                table: "RFQs",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RfqAttachments_RFQs_RfqId",
                table: "RfqAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_RFQs_Customer_CustomerId",
                table: "RFQs");

            migrationBuilder.DropForeignKey(
                name: "FK_RFQs_Products_ProductId",
                table: "RFQs");

            migrationBuilder.DropTable(
                name: "Quotations");

            migrationBuilder.DropTable(
                name: "RfqSupplierAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RFQs",
                table: "RFQs");

            migrationBuilder.DropIndex(
                name: "IX_RFQs_ProductId",
                table: "RFQs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RfqAttachments",
                table: "RfqAttachments");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "RFQs");

            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "RFQs");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "RFQs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "RFQs");

            migrationBuilder.RenameTable(
                name: "RFQs",
                newName: "RFQ");

            migrationBuilder.RenameTable(
                name: "RfqAttachments",
                newName: "RfqAttachment");

            migrationBuilder.RenameIndex(
                name: "IX_RFQs_CustomerId",
                table: "RFQ",
                newName: "IX_RFQ_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_RfqAttachments_RfqId",
                table: "RfqAttachment",
                newName: "IX_RfqAttachment_RfqId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RFQ",
                table: "RFQ",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RfqAttachment",
                table: "RfqAttachment",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_RFQ_Customer_CustomerId",
                table: "RFQ",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RfqAttachment_RFQ_RfqId",
                table: "RfqAttachment",
                column: "RfqId",
                principalTable: "RFQ",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
