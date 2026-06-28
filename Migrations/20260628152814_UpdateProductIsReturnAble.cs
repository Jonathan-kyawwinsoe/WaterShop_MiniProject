using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace water_shop.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductIsReturnAble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SockQuantity",
                table: "Products",
                newName: "StockQuantity");

            migrationBuilder.AlterColumn<bool>(
                name: "IsReturnAble",
                table: "Products",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StockQuantity",
                table: "Products",
                newName: "SockQuantity");

            migrationBuilder.AlterColumn<string>(
                name: "IsReturnAble",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
