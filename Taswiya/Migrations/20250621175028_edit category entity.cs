using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConnectChain.Migrations
{
    /// <inheritdoc />
    public partial class editcategoryentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Suppliers_SupplierId",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "SupplierId",
                table: "Notification",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_SupplierId",
                table: "Notification",
                newName: "IX_Notification_UserId");

            migrationBuilder.AddColumn<string>(
                name: "imageUrl",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Users_UserId",
                table: "Notification",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Users_UserId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "imageUrl",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Notification",
                newName: "SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_UserId",
                table: "Notification",
                newName: "IX_Notification_SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Suppliers_SupplierId",
                table: "Notification",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }
    }
}
