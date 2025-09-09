using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class NewInitialMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "categoryId",
                table: "InventoryModel",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CategoryModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscussionModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    inventoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Text = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscussionModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscussionModel_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscussionModel_InventoryModel_inventoryId",
                        column: x => x.inventoryId,
                        principalTable: "InventoryModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InventoryAccessModel",
                columns: table => new
                {
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    inventoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryAccessModel", x => new { x.inventoryId, x.userId });
                    table.ForeignKey(
                        name: "FK_InventoryAccessModel_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InventoryAccessModel_InventoryModel_inventoryId",
                        column: x => x.inventoryId,
                        principalTable: "InventoryModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    inventoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Price = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemModel_InventoryModel_inventoryId",
                        column: x => x.inventoryId,
                        principalTable: "InventoryModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CategoryModel",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "Book" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "Furniture" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), "Equipment" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), "Other" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryModel_categoryId",
                table: "InventoryModel",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionModel_inventoryId",
                table: "DiscussionModel",
                column: "inventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionModel_userId",
                table: "DiscussionModel",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAccessModel_userId",
                table: "InventoryAccessModel",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemModel_inventoryId",
                table: "ItemModel",
                column: "inventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryModel_CategoryModel_categoryId",
                table: "InventoryModel",
                column: "categoryId",
                principalTable: "CategoryModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryModel_CategoryModel_categoryId",
                table: "InventoryModel");

            migrationBuilder.DropTable(
                name: "CategoryModel");

            migrationBuilder.DropTable(
                name: "DiscussionModel");

            migrationBuilder.DropTable(
                name: "InventoryAccessModel");

            migrationBuilder.DropTable(
                name: "ItemModel");

            migrationBuilder.DropIndex(
                name: "IX_InventoryModel_categoryId",
                table: "InventoryModel");

            migrationBuilder.DropColumn(
                name: "categoryId",
                table: "InventoryModel");
        }
    }
}
