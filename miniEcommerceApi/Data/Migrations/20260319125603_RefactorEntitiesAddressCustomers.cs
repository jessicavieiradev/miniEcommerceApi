using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace miniEcommerceApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorEntitiesAddressCustomers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Costumers",
                newName: "Customers");

            migrationBuilder.RenameTable(
                name: "Adresses",
                newName: "Addresses");

            migrationBuilder.RenameColumn(
                name: "CostumerId",
                table: "Addresses",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "Neiborhood",
                table: "Addresses",
                newName: "Neighborhood");
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Costumers");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "Adresses");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Adresses",
                newName: "CostumerId");

            migrationBuilder.RenameColumn(
                name: "Neighborhood",
                table: "Adresses",
                newName: "Neiborhood");
        }
    }
}
