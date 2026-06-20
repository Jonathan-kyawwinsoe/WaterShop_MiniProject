using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace water_shop.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenToAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreadedAt",
                table: "Admins",
                newName: "CreadeddAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastLoginAt",
                table: "Admins",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiry",
                table: "Admins",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiry",
                table: "Admins");

            migrationBuilder.RenameColumn(
                name: "CreadeddAt",
                table: "Admins",
                newName: "CreadedAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastLoginAt",
                table: "Admins",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
