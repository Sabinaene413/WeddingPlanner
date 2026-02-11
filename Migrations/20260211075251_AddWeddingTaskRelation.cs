using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeddingPlanner.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddWeddingTaskRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WeddingTasks_WeddingId",
                table: "WeddingTasks",
                column: "WeddingId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeddingTasks_Weddings_WeddingId",
                table: "WeddingTasks",
                column: "WeddingId",
                principalTable: "Weddings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeddingTasks_Weddings_WeddingId",
                table: "WeddingTasks");

            migrationBuilder.DropIndex(
                name: "IX_WeddingTasks_WeddingId",
                table: "WeddingTasks");
        }
    }
}
