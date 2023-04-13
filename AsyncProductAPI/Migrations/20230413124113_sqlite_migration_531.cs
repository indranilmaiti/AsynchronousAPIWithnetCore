using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsyncProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class sqlite_migration_531 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ListingRequests",
                table: "ListingRequests");

            migrationBuilder.RenameTable(
                name: "ListingRequests",
                newName: "HeavyProcessingRequests");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HeavyProcessingRequests",
                table: "HeavyProcessingRequests",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HeavyProcessingRequests",
                table: "HeavyProcessingRequests");

            migrationBuilder.RenameTable(
                name: "HeavyProcessingRequests",
                newName: "ListingRequests");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListingRequests",
                table: "ListingRequests",
                column: "Id");
        }
    }
}
