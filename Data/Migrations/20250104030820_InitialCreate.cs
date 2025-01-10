using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dagnys_API.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductName = table.Column<string>(type: "TEXT", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                }
            );

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierId = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Contact = table.Column<string>(type: "TEXT", nullable: true),
                    Phone = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierId);
                }
            );

            migrationBuilder.CreateTable(
                name: "SuppliersProducts",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    SupplierId = table.Column<int>(type: "INTEGER", nullable: false),
                    ArticleNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_SuppliersProducts",
                        x => new { x.ProductId, x.SupplierId }
                    );
                    table.ForeignKey(
                        name: "FK_SuppliersProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_SuppliersProducts_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_SuppliersProducts_SupplierId",
                table: "SuppliersProducts",
                column: "SupplierId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "SuppliersProducts");

            migrationBuilder.DropTable(name: "Products");

            migrationBuilder.DropTable(name: "Suppliers");
        }
    }
}
