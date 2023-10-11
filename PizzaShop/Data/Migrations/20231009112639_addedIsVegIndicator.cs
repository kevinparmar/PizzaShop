using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedIsVegIndicator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartTopping");

            migrationBuilder.AddColumn<bool>(
                name: "IsVegetarian",
                table: "Pizza",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVegetarian",
                table: "Pizza");

            migrationBuilder.CreateTable(
                name: "CartTopping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartDetailId = table.Column<int>(type: "int", nullable: false),
                    ToppingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartTopping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartTopping_CartDetail_CartDetailId",
                        column: x => x.CartDetailId,
                        principalTable: "CartDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartTopping_Topping_ToppingId",
                        column: x => x.ToppingId,
                        principalTable: "Topping",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartTopping_CartDetailId",
                table: "CartTopping",
                column: "CartDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_CartTopping_ToppingId",
                table: "CartTopping",
                column: "ToppingId");
        }
    }
}
