using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OnlineMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedSecurityEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Security",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Password = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Security", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(358));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(364));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(367));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(370));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(372));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(377));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(379));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(381));

            migrationBuilder.UpdateData(
                table: "Filials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(447));

            migrationBuilder.UpdateData(
                table: "Filials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(452));

            migrationBuilder.UpdateData(
                table: "Filials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(453));

            migrationBuilder.UpdateData(
                table: "Filials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(454));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(385));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(398));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(402));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(405));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(407));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(411));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(414));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(416));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(418));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(422));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(424));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(426));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(429));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(431));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(434));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(436));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(438));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 12, 5, 23, 789, DateTimeKind.Utc).AddTicks(442));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Security");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5355));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5361));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5363));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5364));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5365));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5367));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5368));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5369));

            migrationBuilder.UpdateData(
                table: "Filials",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5411));

            migrationBuilder.UpdateData(
                table: "Filials",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5415));

            migrationBuilder.UpdateData(
                table: "Filials",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5415));

            migrationBuilder.UpdateData(
                table: "Filials",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5416));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5374));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5382));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5383));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5385));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5386));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5388));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5390));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5391));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5392));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5395));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5396));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5398));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5399));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5400));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5402));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5403));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5404));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 18, 11, 34, 21, 274, DateTimeKind.Utc).AddTicks(5407));
        }
    }
}
