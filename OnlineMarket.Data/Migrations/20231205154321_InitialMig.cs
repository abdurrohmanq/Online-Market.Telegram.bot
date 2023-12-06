﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnlineMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChatId = table.Column<long>(type: "bigint", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    UserRole = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    StockQuantity = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Sum = table.Column<decimal>(type: "numeric", nullable: false),
                    CartId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    OrderType = table.Column<int>(type: "integer", nullable: false),
                    DeliveryAddress_Longitude = table.Column<double>(type: "double precision", nullable: true),
                    DeliveryAddress_Latitude = table.Column<double>(type: "double precision", nullable: true),
                    DeliveryAddress_HorizontalAccuracy = table.Column<float>(type: "real", nullable: true),
                    DeliveryAddress_LivePeriod = table.Column<int>(type: "integer", nullable: true),
                    DeliveryAddress_Heading = table.Column<int>(type: "integer", nullable: true),
                    DeliveryAddress_ProximityAlertRadius = table.Column<int>(type: "integer", nullable: true),
                    MarketAddress = table.Column<string>(type: "text", nullable: true),
                    PaymentMethod = table.Column<int>(type: "integer", nullable: false),
                    CartId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "IsDelete", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4440), "Category description 1", false, "FastFood", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4448), "Category description 2", false, "Ichimliklar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4449), "Category description 3", false, "Kaboblar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4450), "Category description 4", false, "Suyuq ovqatlar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4451), "Category description 5", false, "Go'shtli ovqatlar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4456), "Category description 6", false, "Shashliklar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4457), "Category description 7", false, "Salatlar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4458), "Category description 8", false, "Xamirli ovqatlar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "IsDelete", "Name", "Price", "StockQuantity", "UpdatedAt" },
                values: new object[,]
                {
                    { 1L, 1L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4463), "Product Desc", false, "HotDog", 10.0m, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2L, 1L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4469), "Product Desc2", false, "Gamburger", 15.0m, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3L, 1L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4471), "Product Desc3", false, "Lavash", 10.0m, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4L, 1L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4472), "Product Desc4", false, "Non burger", 15.0m, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5L, 2L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4474), "Product Desc5", false, "Coco-Cola", 20.0m, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6L, 2L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4476), "Product Desc6", false, "Pepsi", 25.0m, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7L, 2L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4478), "Product Desc7", false, "Fanta", 20.0m, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8L, 2L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4479), "Product Desc8", false, "Kompot", 25.0m, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9L, 3L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4480), "Product Desc5", false, "Tovuq kabob", 20.0m, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10L, 3L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4482), "Product Desc6", false, "Lo'la kabob", 25.0m, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11L, 3L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4483), "Product Desc5", false, "Mol kabob", 20.0m, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12L, 3L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4485), "Product Desc6", false, "Qo'y kabob", 28.0m, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13L, 4L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4486), "Product Desc13", false, "Sho'rva", 30.0m, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14L, 4L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4487), "Product Desc14", false, "Mastava", 28.0m, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15L, 5L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4488), "Product Desc13", false, "Qozon kabob", 30.0m, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16L, 5L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4490), "Product Desc14", false, "Tovuq go'shti", 28.0m, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17L, 6L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4491), "Product Desc13", false, "Qiyma shashlik", 30.0m, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18L, 6L, new DateTime(2023, 12, 5, 15, 43, 20, 842, DateTimeKind.Utc).AddTicks(4493), "Product Desc14", false, "Burda shashlik", 18.0m, 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CartId",
                table: "Orders",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
