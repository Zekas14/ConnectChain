using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConnectChain.Migrations
{
    /// <inheritdoc />
    public partial class editnotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Notification",
                newName: "Body");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Notification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_OrderId",
                table: "Notification",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Order_OrderId",
                table: "Notification",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Order_OrderId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_OrderId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Notification",
                newName: "Message");
        }
    }
}
