using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCaseInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "owner_id",
                table: "case",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "i_x_case_owner_id",
                table: "case",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "f_k_case_user_owner_id",
                table: "case",
                column: "owner_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_case_user_owner_id",
                table: "case");

            migrationBuilder.DropIndex(
                name: "i_x_case_owner_id",
                table: "case");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "case");
        }
    }
}
