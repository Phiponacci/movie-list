using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace movie_list.Migrations
{
    public partial class UpdateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserMovie");

            migrationBuilder.RenameColumn(
                name: "emsVersionId",
                table: "Movies",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_UserId",
                table: "Movies",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_AspNetUsers_UserId",
                table: "Movies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_AspNetUsers_UserId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_UserId",
                table: "Movies");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Movies",
                newName: "emsVersionId");

            migrationBuilder.CreateTable(
                name: "AppUserMovie",
                columns: table => new
                {
                    UsersId = table.Column<string>(type: "TEXT", nullable: false),
                    WatchListemsId = table.Column<string>(type: "TEXT", nullable: false)
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
        }
    }
}
