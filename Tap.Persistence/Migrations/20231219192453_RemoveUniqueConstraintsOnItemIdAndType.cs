using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tap.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueConstraintsOnItemIdAndType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Photo_Type_ItemId",
                table: "Photo");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Photo",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Photo",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_Type_ItemId",
                table: "Photo",
                columns: new[] { "Type", "ItemId" },
                unique: true);
        }
    }
}
