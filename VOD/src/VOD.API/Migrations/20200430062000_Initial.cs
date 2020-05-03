using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VOD.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "vod");

            migrationBuilder.CreateTable(
                name: "Genres",
                schema: "vod",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kinds",
                schema: "vod",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kinds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "vod",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    UserName = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    Email = table.Column<string>(unicode: false, maxLength: 256, nullable: false),
                    SubStartDate = table.Column<DateTime>(nullable: true),
                    SubEndDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                schema: "vod",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    AltTitle = table.Column<string>(maxLength: 150, nullable: false),
                    ReleaseYear = table.Column<DateTime>(nullable: true),
                    Duration = table.Column<int>(nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: false),
                    Season = table.Column<int>(nullable: true),
                    Episode = table.Column<int>(nullable: true),
                    KindId = table.Column<Guid>(nullable: false),
                    GenreId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videos_Genres_GenreId",
                        column: x => x.GenreId,
                        principalSchema: "vod",
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Videos_Kinds_KindId",
                        column: x => x.KindId,
                        principalSchema: "vod",
                        principalTable: "Kinds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Genres_Name",
                schema: "vod",
                table: "Genres",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kinds_Name",
                schema: "vod",
                table: "Kinds",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                schema: "vod",
                table: "Users",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_AltTitle",
                schema: "vod",
                table: "Videos",
                column: "AltTitle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_GenreId",
                schema: "vod",
                table: "Videos",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_KindId",
                schema: "vod",
                table: "Videos",
                column: "KindId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users",
                schema: "vod");

            migrationBuilder.DropTable(
                name: "Videos",
                schema: "vod");

            migrationBuilder.DropTable(
                name: "Genres",
                schema: "vod");

            migrationBuilder.DropTable(
                name: "Kinds",
                schema: "vod");
        }
    }
}
