using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoggoDex.Migrations
{
    /// <inheritdoc />
    public partial class ExpandAndAddDogsAndReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dog_DogOwners_DogOwnerId",
                table: "Dog");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_BusinessOwners_BusinessOwnerId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Dog_DogId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dog",
                table: "Dog");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameTable(
                name: "Dog",
                newName: "Dogs");

            migrationBuilder.RenameIndex(
                name: "IX_Review_DogId",
                table: "Reviews",
                newName: "IX_Reviews_DogId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_BusinessOwnerId",
                table: "Reviews",
                newName: "IX_Reviews_BusinessOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Dog_DogOwnerId",
                table: "Dogs",
                newName: "IX_Dogs_DogOwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dogs",
                table: "Dogs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dogs_DogOwners_DogOwnerId",
                table: "Dogs",
                column: "DogOwnerId",
                principalTable: "DogOwners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_BusinessOwners_BusinessOwnerId",
                table: "Reviews",
                column: "BusinessOwnerId",
                principalTable: "BusinessOwners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Dogs_DogId",
                table: "Reviews",
                column: "DogId",
                principalTable: "Dogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dogs_DogOwners_DogOwnerId",
                table: "Dogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_BusinessOwners_BusinessOwnerId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Dogs_DogId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dogs",
                table: "Dogs");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameTable(
                name: "Dogs",
                newName: "Dog");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_DogId",
                table: "Review",
                newName: "IX_Review_DogId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_BusinessOwnerId",
                table: "Review",
                newName: "IX_Review_BusinessOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Dogs_DogOwnerId",
                table: "Dog",
                newName: "IX_Dog_DogOwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dog",
                table: "Dog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dog_DogOwners_DogOwnerId",
                table: "Dog",
                column: "DogOwnerId",
                principalTable: "DogOwners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_BusinessOwners_BusinessOwnerId",
                table: "Review",
                column: "BusinessOwnerId",
                principalTable: "BusinessOwners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Dog_DogId",
                table: "Review",
                column: "DogId",
                principalTable: "Dog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
