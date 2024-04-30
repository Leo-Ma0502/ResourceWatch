using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResourceWatcher.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImageModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Timestamp",
                table: "Images",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Images");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Timestamp",
                table: "Images",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
