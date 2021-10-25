using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StockMachine.Trade.Migrations
{
    public partial class commodities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Commodities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ChangePercent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Volume = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    High = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Low = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commodities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarketComplianceEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientRelationshipServicesEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Coordinates = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeBreakDown",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubSidiaryMarkets = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExchangeTimeZonesInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarketCapitilization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExchangeCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeBreakDown", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TopMovers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Change = table.Column<double>(type: "float", nullable: false),
                    ChangeFactor = table.Column<double>(type: "float", nullable: false),
                    ChangeFactorType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopMovers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradingSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreTradingSession = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoreTradingSessions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendedHours = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradingSchedule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exchanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Market = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Overview = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExchangeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExchangeSymbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarketCap = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hours = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TradingScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExchangeBreakDownId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    History = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeZone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MicCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    contactInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exchanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exchanges_ContactInfo_contactInfoId",
                        column: x => x.contactInfoId,
                        principalTable: "ContactInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Exchanges_ExchangeBreakDown_ExchangeBreakDownId",
                        column: x => x.ExchangeBreakDownId,
                        principalTable: "ExchangeBreakDown",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Exchanges_TradingSchedule_TradingScheduleId",
                        column: x => x.TradingScheduleId,
                        principalTable: "TradingSchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exchanges_contactInfoId",
                table: "Exchanges",
                column: "contactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Exchanges_ExchangeBreakDownId",
                table: "Exchanges",
                column: "ExchangeBreakDownId");

            migrationBuilder.CreateIndex(
                name: "IX_Exchanges_TradingScheduleId",
                table: "Exchanges",
                column: "TradingScheduleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commodities");

            migrationBuilder.DropTable(
                name: "Exchanges");

            migrationBuilder.DropTable(
                name: "TopMovers");

            migrationBuilder.DropTable(
                name: "ContactInfo");

            migrationBuilder.DropTable(
                name: "ExchangeBreakDown");

            migrationBuilder.DropTable(
                name: "TradingSchedule");
        }
    }
}
