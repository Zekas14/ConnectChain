using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConnectChain.Migrations
{
    /// <inheritdoc />
    public partial class PaymentTermsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ValidUntil",
                table: "Quotations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryTimeInDays",
                table: "Quotations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DeliveryFee",
                table: "Quotations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryTerm",
                table: "Quotations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PaymentTermId",
                table: "Quotations",
                type: "int",
                nullable: false,
                defaultValue: 0);


            migrationBuilder.CreateTable(
                name: "PaymentTerms",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTerms", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_PaymentTermId",
                table: "Quotations",
                column: "PaymentTermId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotations_PaymentTerms_PaymentTermId",
                table: "Quotations",
                column: "PaymentTermId",
                principalTable: "PaymentTerms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
         

            migrationBuilder.DropTable(
                name: "PaymentTerms");

            migrationBuilder.DropIndex(
                name: "IX_Quotations_PaymentTermId",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "DeliveryFee",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "DeliveryTerm",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "PaymentTermId",
                table: "Quotations");


            migrationBuilder.AlterColumn<DateTime>(
                name: "ValidUntil",
                table: "Quotations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryTimeInDays",
                table: "Quotations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

           
        }
    }
}
