using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tap.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerForHotel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Hotel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Hotel_UserId",
                table: "Hotel",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotel_User_UserId",
                table: "Hotel",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotel_User_UserId",
                table: "Hotel");

            migrationBuilder.DropIndex(
                name: "IX_Hotel_UserId",
                table: "Hotel");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Hotel");
        }
    }
}
