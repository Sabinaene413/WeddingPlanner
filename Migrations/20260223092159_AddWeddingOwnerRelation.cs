using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeddingPlanner.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddWeddingOwnerRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Weddings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Weddings_OwnerId",
                table: "Weddings",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weddings_Users_OwnerId",
                table: "Weddings",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weddings_Users_OwnerId",
                table: "Weddings");

            migrationBuilder.DropIndex(
                name: "IX_Weddings_OwnerId",
                table: "Weddings");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Weddings");
        }
    }
}
