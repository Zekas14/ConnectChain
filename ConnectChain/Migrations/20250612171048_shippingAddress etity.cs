using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConnectChain.Migrations
{
    /// <inheritdoc />
    public partial class shippingAddressetity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplierPaymentMethods");

            migrationBuilder.AddColumn<string>(
                name: "SupplierId",
                table: "PaymentMethods",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserPaymentMethod",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentMethodID = table.Column<int>(type: "int", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPaymentMethod", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserPaymentMethod_PaymentMethods_PaymentMethodID",
                        column: x => x.PaymentMethodID,
                        principalTable: "PaymentMethods",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPaymentMethod_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserShippingAddress",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apartment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserShippingAddress", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserShippingAddress_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_SupplierId",
                table: "PaymentMethods",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPaymentMethod_PaymentMethodID",
                table: "UserPaymentMethod",
                column: "PaymentMethodID");

            migrationBuilder.CreateIndex(
                name: "IX_UserPaymentMethod_UserID",
                table: "UserPaymentMethod",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserShippingAddress_UserId",
                table: "UserShippingAddress",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethods_Suppliers_SupplierId",
                table: "PaymentMethods",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethods_Suppliers_SupplierId",
                table: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "UserPaymentMethod");

            migrationBuilder.DropTable(
                name: "UserShippingAddress");

            migrationBuilder.DropIndex(
                name: "IX_PaymentMethods_SupplierId",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "PaymentMethods");

            migrationBuilder.CreateTable(
                name: "SupplierPaymentMethods",
                columns: table => new
                {
                    SupplierID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentMethodID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierPaymentMethods", x => new { x.SupplierID, x.PaymentMethodID });
                    table.ForeignKey(
                        name: "FK_SupplierPaymentMethods_PaymentMethods_PaymentMethodID",
                        column: x => x.PaymentMethodID,
                        principalTable: "PaymentMethods",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierPaymentMethods_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplierPaymentMethods_PaymentMethodID",
                table: "SupplierPaymentMethods",
                column: "PaymentMethodID");
        }
    }
}
