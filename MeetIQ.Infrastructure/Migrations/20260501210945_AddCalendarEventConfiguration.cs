using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetIQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCalendarEventConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarEvents_AspNetUsers_OwnerId",
                table: "CalendarEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_CalendarEvents_Meetings_MeetingId",
                table: "CalendarEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_CalendarEvents_CalendarEventId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_CalendarEvents_MeetingId",
                table: "CalendarEvents");

            migrationBuilder.AddColumn<Guid>(
                name: "CalendarEventId1",
                table: "Notes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "CalendarEvents",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "CalendarEvents",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CalendarEvents",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "CalendarEvents",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: false,
                defaultValue: "#3B82F6",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "CalendarEvents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MeetingId1",
                table: "CalendarEvents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_CalendarEventId1",
                table: "Notes",
                column: "CalendarEventId1");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvents_ApplicationUserId",
                table: "CalendarEvents",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvents_MeetingId",
                table: "CalendarEvents",
                column: "MeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvents_MeetingId1",
                table: "CalendarEvents",
                column: "MeetingId1",
                unique: true,
                filter: "[MeetingId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEvents_AspNetUsers_ApplicationUserId",
                table: "CalendarEvents",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEvents_AspNetUsers_OwnerId",
                table: "CalendarEvents",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEvents_Meetings_MeetingId",
                table: "CalendarEvents",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEvents_Meetings_MeetingId1",
                table: "CalendarEvents",
                column: "MeetingId1",
                principalTable: "Meetings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_CalendarEvents_CalendarEventId",
                table: "Notes",
                column: "CalendarEventId",
                principalTable: "CalendarEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_CalendarEvents_CalendarEventId1",
                table: "Notes",
                column: "CalendarEventId1",
                principalTable: "CalendarEvents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarEvents_AspNetUsers_ApplicationUserId",
                table: "CalendarEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_CalendarEvents_AspNetUsers_OwnerId",
                table: "CalendarEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_CalendarEvents_Meetings_MeetingId",
                table: "CalendarEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_CalendarEvents_Meetings_MeetingId1",
                table: "CalendarEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_CalendarEvents_CalendarEventId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_CalendarEvents_CalendarEventId1",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_CalendarEventId1",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_CalendarEvents_ApplicationUserId",
                table: "CalendarEvents");

            migrationBuilder.DropIndex(
                name: "IX_CalendarEvents_MeetingId",
                table: "CalendarEvents");

            migrationBuilder.DropIndex(
                name: "IX_CalendarEvents_MeetingId1",
                table: "CalendarEvents");

            migrationBuilder.DropColumn(
                name: "CalendarEventId1",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "CalendarEvents");

            migrationBuilder.DropColumn(
                name: "MeetingId1",
                table: "CalendarEvents");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "CalendarEvents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "CalendarEvents",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "CalendarEvents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "CalendarEvents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(7)",
                oldMaxLength: 7,
                oldDefaultValue: "#3B82F6");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvents_MeetingId",
                table: "CalendarEvents",
                column: "MeetingId",
                unique: true,
                filter: "[MeetingId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEvents_AspNetUsers_OwnerId",
                table: "CalendarEvents",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEvents_Meetings_MeetingId",
                table: "CalendarEvents",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_CalendarEvents_CalendarEventId",
                table: "Notes",
                column: "CalendarEventId",
                principalTable: "CalendarEvents",
                principalColumn: "Id");
        }
    }
}
