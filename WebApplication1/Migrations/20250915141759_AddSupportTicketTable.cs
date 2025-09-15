using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddSupportTicketTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryModel_CategoryModel_categoryId",
                table: "InventoryModel");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemModel_InventoryModel_inventoryId",
                table: "ItemModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemModel",
                table: "ItemModel");

            migrationBuilder.DropIndex(
                name: "IX_InventoryModel_categoryId",
                table: "InventoryModel");

            migrationBuilder.RenameTable(
                name: "ItemModel",
                newName: "itemModel");

            migrationBuilder.RenameIndex(
                name: "IX_ItemModel_inventoryId",
                table: "itemModel",
                newName: "IX_itemModel_inventoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_itemModel",
                table: "itemModel",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ApiTokenModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InventoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiTokenModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiTokenModel_InventoryModel_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "InventoryModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupportTickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    InventoryTitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PageLink = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    AdminEmails = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CloudFileUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsProcessed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTickets", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiTokenModel_InventoryId",
                table: "ApiTokenModel",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiTokenModel_Token",
                table: "ApiTokenModel",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_CreatedAt",
                table: "SupportTickets",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_IsProcessed",
                table: "SupportTickets",
                column: "IsProcessed");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_ReportedBy",
                table: "SupportTickets",
                column: "ReportedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_itemModel_InventoryModel_inventoryId",
                table: "itemModel",
                column: "inventoryId",
                principalTable: "InventoryModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_itemModel_InventoryModel_inventoryId",
                table: "itemModel");

            migrationBuilder.DropTable(
                name: "ApiTokenModel");

            migrationBuilder.DropTable(
                name: "SupportTickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_itemModel",
                table: "itemModel");

            migrationBuilder.RenameTable(
                name: "itemModel",
                newName: "ItemModel");

            migrationBuilder.RenameIndex(
                name: "IX_itemModel_inventoryId",
                table: "ItemModel",
                newName: "IX_ItemModel_inventoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemModel",
                table: "ItemModel",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryModel_categoryId",
                table: "InventoryModel",
                column: "categoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryModel_CategoryModel_categoryId",
                table: "InventoryModel",
                column: "categoryId",
                principalTable: "CategoryModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemModel_InventoryModel_inventoryId",
                table: "ItemModel",
                column: "inventoryId",
                principalTable: "InventoryModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
