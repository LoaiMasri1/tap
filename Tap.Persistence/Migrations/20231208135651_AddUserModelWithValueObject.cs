using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tap.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserModelWithValueObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Users");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        Name = table.Column<string>(
                            type: "nvarchar(50)",
                            maxLength: 50,
                            nullable: false
                        ),
                        Email = table.Column<string>(
                            type: "nvarchar(254)",
                            maxLength: 254,
                            nullable: false
                        ),
                        Password = table.Column<string>(
                            type: "nvarchar(128)",
                            maxLength: 128,
                            nullable: false
                        ),
                        Role = table.Column<int>(type: "int", nullable: false),
                        CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                        UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "User");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                        Email = table.Column<string>(
                            type: "nvarchar(256)",
                            maxLength: 256,
                            nullable: false
                        ),
                        Name = table.Column<string>(
                            type: "nvarchar(50)",
                            maxLength: 50,
                            nullable: false
                        ),
                        Password = table.Column<string>(
                            type: "nvarchar(50)",
                            maxLength: 50,
                            nullable: false
                        ),
                        UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                }
            );
        }
    }
}
