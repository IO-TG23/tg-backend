using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TG.Backend.Migrations
{
    /// <inheritdoc />
    public partial class CreateRelationBetweenOfferAndBlob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OfferId",
                table: "Blobs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Blobs_OfferId",
                table: "Blobs",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blobs_Offers_OfferId",
                table: "Blobs",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blobs_Offers_OfferId",
                table: "Blobs");

            migrationBuilder.DropIndex(
                name: "IX_Blobs_OfferId",
                table: "Blobs");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "Blobs");
        }
    }
}
