using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movie_list.Migrations
{
    public partial class AddWatchListToAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PosterImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosterImage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    emsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    emsVersionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    releaseDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    posterImageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.emsId);
                    table.ForeignKey(
                        name: "FK_Movies_PosterImage_posterImageId",
                        column: x => x.posterImageId,
                        principalTable: "PosterImage",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppUserMovie",
                columns: table => new
                {
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WatchListemsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserMovie", x => new { x.UsersId, x.WatchListemsId });
                    table.ForeignKey(
                        name: "FK_AppUserMovie_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserMovie_Movies_WatchListemsId",
                        column: x => x.WatchListemsId,
                        principalTable: "Movies",
                        principalColumn: "emsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserMovie_WatchListemsId",
                table: "AppUserMovie",
                column: "WatchListemsId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_posterImageId",
                table: "Movies",
                column: "posterImageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserMovie");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "PosterImage");
        }
    }
}
