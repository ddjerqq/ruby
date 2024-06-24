using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_item__item_type_type_id",
                table: "item");

            migrationBuilder.CreateTable(
                name: "case",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    image_url = table.Column<string>(type: "TEXT", nullable: false),
                    created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    created_by = table.Column<string>(type: "TEXT", nullable: true),
                    last_modified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    last_modified_by = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_case", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "case_drop",
                columns: table => new
                {
                    item_type_id = table.Column<string>(type: "TEXT", nullable: false),
                    case_id = table.Column<string>(type: "TEXT", nullable: false),
                    drop_chance = table.Column<decimal>(type: "TEXT", nullable: false),
                    drop_price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_case_drop", x => new { x.case_id, x.item_type_id });
                    table.ForeignKey(
                        name: "f_k_case_drop_case_case_id",
                        column: x => x.case_id,
                        principalTable: "case",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_case_drop_item_type_item_type_id",
                        column: x => x.item_type_id,
                        principalTable: "item_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_case_drop_item_type_id",
                table: "case_drop",
                column: "item_type_id");

            migrationBuilder.AddForeignKey(
                name: "f_k_item__item_types_type_id",
                table: "item",
                column: "type_id",
                principalTable: "item_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_item__item_types_type_id",
                table: "item");

            migrationBuilder.DropTable(
                name: "case_drop");

            migrationBuilder.DropTable(
                name: "case");

            migrationBuilder.AddForeignKey(
                name: "f_k_item__item_type_type_id",
                table: "item",
                column: "type_id",
                principalTable: "item_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
