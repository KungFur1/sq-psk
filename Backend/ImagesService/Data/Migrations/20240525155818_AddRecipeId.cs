using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImagesService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasRecipe",
                table: "ImagesMetaData");

            migrationBuilder.AddColumn<Guid>(
                name: "RecipeId",
                table: "ImagesMetaData",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "ImagesMetaData");

            migrationBuilder.AddColumn<bool>(
                name: "HasRecipe",
                table: "ImagesMetaData",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
