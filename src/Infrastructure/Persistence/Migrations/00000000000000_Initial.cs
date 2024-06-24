using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "case_type",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    image_url = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    created_by = table.Column<string>(type: "TEXT", nullable: true),
                    last_modified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    last_modified_by = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_case_type", x => x.id);
                });

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

            migrationBuilder.CreateTable(
                name: "outbox_message",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    type = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    content = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: false),
                    occured_on_utc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    processed_on_utc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    error = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_outbox_message", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    username = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    email = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    password_hash = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    level = table.Column<int>(type: "INTEGER", nullable: false),
                    wallet = table.Column<decimal>(type: "TEXT", nullable: false),
                    created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    created_by = table.Column<string>(type: "TEXT", nullable: true),
                    last_modified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    last_modified_by = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "case_drop",
                columns: table => new
                {
                    item_type_id = table.Column<string>(type: "TEXT", nullable: false),
                    case_type_id = table.Column<string>(type: "TEXT", nullable: false),
                    drop_chance = table.Column<decimal>(type: "TEXT", nullable: false),
                    drop_price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_case_drop", x => new { x.case_type_id, x.item_type_id });
                    table.ForeignKey(
                        name: "f_k_case_drop_case_type_case_type_id",
                        column: x => x.case_type_id,
                        principalTable: "case_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_case_drop_item_type_item_type_id",
                        column: x => x.item_type_id,
                        principalTable: "item_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "case",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    case_type_id = table.Column<string>(type: "TEXT", nullable: false),
                    is_opened = table.Column<bool>(type: "INTEGER", nullable: false),
                    opened_on = table.Column<DateTime>(type: "TEXT", nullable: true),
                    owner_id = table.Column<string>(type: "TEXT", nullable: false),
                    created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    created_by = table.Column<string>(type: "TEXT", nullable: true),
                    last_modified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    last_modified_by = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_case", x => x.id);
                    table.ForeignKey(
                        name: "f_k_case__case_type_case_type_id",
                        column: x => x.case_type_id,
                        principalTable: "case_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_case_user_owner_id",
                        column: x => x.owner_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    item_type_id = table.Column<string>(type: "TEXT", nullable: false),
                    owner_id = table.Column<string>(type: "TEXT", nullable: false),
                    is_stat_track = table.Column<bool>(type: "INTEGER", nullable: false),
                    quality = table.Column<float>(type: "REAL", nullable: false),
                    created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    created_by = table.Column<string>(type: "TEXT", nullable: true),
                    last_modified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    last_modified_by = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_item", x => x.id);
                    table.ForeignKey(
                        name: "f_k_item__item_types_item_type_id",
                        column: x => x.item_type_id,
                        principalTable: "item_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_item_user_owner_id",
                        column: x => x.owner_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_case_case_type_id",
                table: "case",
                column: "case_type_id");

            migrationBuilder.CreateIndex(
                name: "i_x_case_owner_id",
                table: "case",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "i_x_case_drop_item_type_id",
                table: "case_drop",
                column: "item_type_id");

            migrationBuilder.CreateIndex(
                name: "i_x_item_item_type_id",
                table: "item",
                column: "item_type_id");

            migrationBuilder.CreateIndex(
                name: "i_x_item_owner_id",
                table: "item",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "i_x_user_email",
                table: "user",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_user_username",
                table: "user",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "case");

            migrationBuilder.DropTable(
                name: "case_drop");

            migrationBuilder.DropTable(
                name: "item");

            migrationBuilder.DropTable(
                name: "outbox_message");

            migrationBuilder.DropTable(
                name: "case_type");

            migrationBuilder.DropTable(
                name: "item_type");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
