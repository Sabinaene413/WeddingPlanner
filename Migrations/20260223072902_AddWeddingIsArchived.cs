using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeddingPlanner.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddWeddingIsArchived : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Weddings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Weddings");
        }
    }
}
