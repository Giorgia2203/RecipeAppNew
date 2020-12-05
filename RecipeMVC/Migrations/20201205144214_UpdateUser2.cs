using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeMVC.Migrations
{
    public partial class UpdateUser2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteRecipe_AspNetUsers_UserId1",
                table: "FavouriteRecipe");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_AspNetUsers_UserId1",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_AspNetUsers_UserId",
                table: "Recipe");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_AspNetUsers_UserId1",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Review_UserId1",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Recipe_UserId",
                table: "Recipe");

            migrationBuilder.DropIndex(
                name: "IX_Image_UserId1",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_FavouriteRecipe_UserId1",
                table: "FavouriteRecipe");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "FavouriteRecipe");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Review",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Recipe",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Image",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "FavouriteRecipe",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Review_AppUserId",
                table: "Review",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_AppUserId",
                table: "Recipe",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_AppUserId",
                table: "Image",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteRecipe_AppUserId",
                table: "FavouriteRecipe",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteRecipe_AspNetUsers_AppUserId",
                table: "FavouriteRecipe",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_AspNetUsers_AppUserId",
                table: "Image",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_AspNetUsers_AppUserId",
                table: "Recipe",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_AspNetUsers_AppUserId",
                table: "Review",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteRecipe_AspNetUsers_AppUserId",
                table: "FavouriteRecipe");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_AspNetUsers_AppUserId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_AspNetUsers_AppUserId",
                table: "Recipe");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_AspNetUsers_AppUserId",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Review_AppUserId",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Recipe_AppUserId",
                table: "Recipe");

            migrationBuilder.DropIndex(
                name: "IX_Image_AppUserId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_FavouriteRecipe_AppUserId",
                table: "FavouriteRecipe");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "FavouriteRecipe");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Review",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Recipe",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Image",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "FavouriteRecipe",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Review_UserId1",
                table: "Review",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_UserId",
                table: "Recipe",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_UserId1",
                table: "Image",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteRecipe_UserId1",
                table: "FavouriteRecipe",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteRecipe_AspNetUsers_UserId1",
                table: "FavouriteRecipe",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_AspNetUsers_UserId1",
                table: "Image",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_AspNetUsers_UserId",
                table: "Recipe",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_AspNetUsers_UserId1",
                table: "Review",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
