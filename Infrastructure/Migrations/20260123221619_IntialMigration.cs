using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IntialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "apartments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Address_Country = table.Column<string>(type: "text", nullable: false),
                    Address_State = table.Column<string>(type: "text", nullable: false),
                    Address_ZipCode = table.Column<string>(type: "text", nullable: false),
                    Address_City = table.Column<string>(type: "text", nullable: false),
                    Address_Street = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Price_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Price_Currency = table.Column<string>(type: "text", nullable: false),
                    CleaningFee_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    CleaningFee_Currency = table.Column<string>(type: "text", nullable: false),
                    LastBookOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Amenities = table.Column<int[]>(type: "integer[]", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_apartments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Duration_StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Duration_EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PriceForPeriod_Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PriceForPeriod_Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    AmenitiesUpCharge_Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AmenitiesUpCharge_Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CleaningFee_Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CleaningFee_Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TotalPrice_Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalPrice_Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConfirmedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RejectedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CanceledOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bookings_apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bookings_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bookings_ApartmentId",
                table: "bookings",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_UserId",
                table: "bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "apartments");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
