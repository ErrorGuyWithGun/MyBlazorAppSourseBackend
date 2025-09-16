using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class ddtemagelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InventoryId",
                table: "itemModel",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ItemTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemTags_itemModel_ItemId",
                        column: x => x.ItemId,
                        principalTable: "itemModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_itemModel_InventoryId",
                table: "itemModel",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTags_ItemId_TagId",
                table: "ItemTags",
                columns: new[] { "ItemId", "TagId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemTags_TagId",
                table: "ItemTags",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_itemModel_InventoryModel_InventoryId",
                table: "itemModel",
                column: "InventoryId",
                principalTable: "InventoryModel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_itemModel_InventoryModel_InventoryId",
                table: "itemModel");

            migrationBuilder.DropTable(
                name: "ItemTags");

            migrationBuilder.DropIndex(
                name: "IX_itemModel_InventoryId",
                table: "itemModel");

            migrationBuilder.DropColumn(
                name: "InventoryId",
                table: "itemModel");
        }
    }
}
