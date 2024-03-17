using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineShop.Data.Migrations
{
    public partial class PaymentAndOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderCondition",
                table: "OrderDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentCondition",
                table: "OrderDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethods",
                table: "OrderDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresPreservation",
                table: "OrderDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StripeSessionId",
                table: "OrderDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderCondition",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "PaymentCondition",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "PaymentMethods",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "RequiresPreservation",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "StripeSessionId",
                table: "OrderDetails");
        }
    }
}
