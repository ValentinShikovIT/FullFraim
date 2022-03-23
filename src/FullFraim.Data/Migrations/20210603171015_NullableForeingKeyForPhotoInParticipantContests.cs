using Microsoft.EntityFrameworkCore.Migrations;

namespace FullFraim.Data.Migrations
{
    public partial class NullableForeingKeyForPhotoInParticipantContests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParticipantContests_Photos_PhotoId",
                table: "ParticipantContests");

            migrationBuilder.DropIndex(
                name: "IX_ParticipantContests_PhotoId",
                table: "ParticipantContests");

            migrationBuilder.AlterColumn<int>(
                name: "PhotoId",
                table: "ParticipantContests",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantContests_PhotoId",
                table: "ParticipantContests",
                column: "PhotoId",
                unique: true,
                filter: "[PhotoId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ParticipantContests_Photos_PhotoId",
                table: "ParticipantContests",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParticipantContests_Photos_PhotoId",
                table: "ParticipantContests");

            migrationBuilder.DropIndex(
                name: "IX_ParticipantContests_PhotoId",
                table: "ParticipantContests");

            migrationBuilder.AlterColumn<int>(
                name: "PhotoId",
                table: "ParticipantContests",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantContests_PhotoId",
                table: "ParticipantContests",
                column: "PhotoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ParticipantContests_Photos_PhotoId",
                table: "ParticipantContests",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
