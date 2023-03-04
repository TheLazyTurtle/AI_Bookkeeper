using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataManager.Migrations
{
    /// <inheritdoc />
    public partial class AddedCategoryAddingType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategorySelection",
                table: "Transactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategorySelection",
                table: "Transactions");
        }
    }
}
