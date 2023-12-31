using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tap.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomAndDiscountEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Room",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        Number = table.Column<int>(type: "int", nullable: false),
                        Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                        Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        DiscountedPrice = table.Column<decimal>(
                            type: "decimal(18,2)",
                            nullable: false
                        ),
                        Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                        CapacityOfAdults = table.Column<int>(type: "int", nullable: false),
                        CapacityOfChildren = table.Column<int>(type: "int", nullable: false),
                        HotelId = table.Column<int>(type: "int", nullable: false),
                        CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                        UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Room_Hotel_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Discount",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        Name = table.Column<string>(
                            type: "nvarchar(100)",
                            maxLength: 100,
                            nullable: false
                        ),
                        Description = table.Column<string>(
                            type: "nvarchar(500)",
                            maxLength: 500,
                            nullable: false
                        ),
                        DiscountPercentage = table.Column<decimal>(
                            type: "decimal(18,2)",
                            nullable: false
                        ),
                        StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                        EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                        CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                        UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                        RoomId = table.Column<int>(type: "int", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discount_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Discount_RoomId",
                table: "Discount",
                column: "RoomId"
            );

            migrationBuilder.CreateIndex(name: "IX_Room_HotelId", table: "Room", column: "HotelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Discount");

            migrationBuilder.DropTable(name: "Room");
        }
    }
}
