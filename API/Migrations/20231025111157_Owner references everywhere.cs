using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class Ownerreferenceseverywhere : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Workers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Buildings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_OwnerId",
                table: "Workers",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_OwnerId",
                table: "Buildings",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_AspNetUsers_OwnerId",
                table: "Buildings",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_AspNetUsers_OwnerId",
                table: "Workers",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_AspNetUsers_OwnerId",
                table: "Buildings");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_AspNetUsers_OwnerId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_OwnerId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_OwnerId",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Buildings");
        }
    }
}
