using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetIQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMeetingInvitationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingInvitations_AspNetUsers_InvitedById",
                table: "MeetingInvitations");

            migrationBuilder.DropForeignKey(
                name: "FK_MeetingInvitations_AspNetUsers_InvitedUserId",
                table: "MeetingInvitations");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_MeetingInvitations_InvitedById",
                table: "MeetingInvitations");

            migrationBuilder.DropColumn(
                name: "InvitedById",
                table: "MeetingInvitations");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Notifications",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Notifications",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ActionUrl",
                table: "Notifications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InvitedByUserId",
                table: "MeetingInvitations",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_CreatedAt",
                table: "Notifications",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_IsRead_IsDeleted",
                table: "Notifications",
                columns: new[] { "UserId", "IsRead", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_MeetingInvitations_InvitedByUserId",
                table: "MeetingInvitations",
                column: "InvitedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingInvitations_AspNetUsers_InvitedByUserId",
                table: "MeetingInvitations",
                column: "InvitedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingInvitations_AspNetUsers_InvitedUserId",
                table: "MeetingInvitations",
                column: "InvitedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingInvitations_AspNetUsers_InvitedByUserId",
                table: "MeetingInvitations");

            migrationBuilder.DropForeignKey(
                name: "FK_MeetingInvitations_AspNetUsers_InvitedUserId",
                table: "MeetingInvitations");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId_CreatedAt",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId_IsRead_IsDeleted",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_MeetingInvitations_InvitedByUserId",
                table: "MeetingInvitations");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "ActionUrl",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InvitedByUserId",
                table: "MeetingInvitations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "InvitedById",
                table: "MeetingInvitations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingInvitations_InvitedById",
                table: "MeetingInvitations",
                column: "InvitedById");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingInvitations_AspNetUsers_InvitedById",
                table: "MeetingInvitations",
                column: "InvitedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingInvitations_AspNetUsers_InvitedUserId",
                table: "MeetingInvitations",
                column: "InvitedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
