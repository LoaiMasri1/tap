﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tap.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
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
                            type: "nvarchar(256)",
                            maxLength: 256,
                            nullable: false
                        ),
                        Password = table.Column<string>(
                            type: "nvarchar(50)",
                            maxLength: 50,
                            nullable: false
                        ),
                        CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                        UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Users");
        }
    }
}
