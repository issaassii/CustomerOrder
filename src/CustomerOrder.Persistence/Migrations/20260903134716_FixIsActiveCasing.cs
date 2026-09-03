using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerOrder.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixIsActiveCasing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "Users",
                newName: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Users",
                newName: "isActive");
        }
    }
}
