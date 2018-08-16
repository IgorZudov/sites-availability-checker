using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SitesChecker.DataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AvailabilityId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    UpdateDelay = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteAvailabilities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsAvailable = table.Column<bool>(nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(nullable: false),
                    SiteId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteAvailabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteAvailabilities_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SiteAvailabilities_SiteId",
                table: "SiteAvailabilities",
                column: "SiteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sites_AvailabilityId",
                table: "Sites",
                column: "AvailabilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sites_SiteAvailabilities_AvailabilityId",
                table: "Sites",
                column: "AvailabilityId",
                principalTable: "SiteAvailabilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SiteAvailabilities_Sites_SiteId",
                table: "SiteAvailabilities");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Sites");

            migrationBuilder.DropTable(
                name: "SiteAvailabilities");
        }
    }
}
