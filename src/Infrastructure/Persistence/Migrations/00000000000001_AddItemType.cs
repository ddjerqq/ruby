using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddItemType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "battle_scarred_img",
                table: "item");

            migrationBuilder.DropColumn(
                name: "factory_new_img",
                table: "item");

            migrationBuilder.DropColumn(
                name: "field_tested_img",
                table: "item");

            migrationBuilder.DropColumn(
                name: "minimal_wear_img",
                table: "item");

            migrationBuilder.DropColumn(
                name: "name",
                table: "item");

            migrationBuilder.DropColumn(
                name: "rarity",
                table: "item");

            migrationBuilder.RenameColumn(
                name: "well_worn_img",
                table: "item",
                newName: "type_id");

            migrationBuilder.CreateTable(
                name: "item_type",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    description = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    quality_min = table.Column<float>(type: "REAL", nullable: false),
                    quality_max = table.Column<float>(type: "REAL", nullable: false),
                    stat_track_available = table.Column<bool>(type: "INTEGER", nullable: false),
                    rarity = table.Column<string>(type: "TEXT", nullable: false),
                    battle_scarred_img = table.Column<string>(type: "TEXT", nullable: false),
                    factory_new_img = table.Column<string>(type: "TEXT", nullable: false),
                    field_tested_img = table.Column<string>(type: "TEXT", nullable: false),
                    minimal_wear_img = table.Column<string>(type: "TEXT", nullable: false),
                    well_worn_img = table.Column<string>(type: "TEXT", nullable: false),
                    created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    created_by = table.Column<string>(type: "TEXT", nullable: true),
                    last_modified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    last_modified_by = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_item_type", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_item_type_id",
                table: "item",
                column: "type_id");

            migrationBuilder.AddForeignKey(
                name: "f_k_item__item_type_type_id",
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
                name: "f_k_item__item_type_type_id",
                table: "item");

            migrationBuilder.DropTable(
                name: "item_type");

            migrationBuilder.DropIndex(
                name: "i_x_item_type_id",
                table: "item");

            migrationBuilder.RenameColumn(
                name: "type_id",
                table: "item",
                newName: "well_worn_img");

            migrationBuilder.AddColumn<string>(
                name: "battle_scarred_img",
                table: "item",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "factory_new_img",
                table: "item",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "field_tested_img",
                table: "item",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "minimal_wear_img",
                table: "item",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "item",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "rarity",
                table: "item",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
