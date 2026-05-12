using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetIQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTranscriptLanguageAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProcessed",
                table: "MeetingTranscripts");

            migrationBuilder.DropColumn(
                name: "RawText",
                table: "MeetingTranscripts");

            migrationBuilder.RenameColumn(
                name: "AudioFileUrl",
                table: "MeetingTranscripts",
                newName: "Text");

            migrationBuilder.AddColumn<string>(
                name: "AudioFilePath",
                table: "MeetingTranscripts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "MeetingTranscripts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "MeetingTranscripts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "MeetingTranscripts",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudioFilePath",
                table: "MeetingTranscripts");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "MeetingTranscripts");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MeetingTranscripts");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "MeetingTranscripts");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "MeetingTranscripts",
                newName: "AudioFileUrl");

            migrationBuilder.AddColumn<bool>(
                name: "IsProcessed",
                table: "MeetingTranscripts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RawText",
                table: "MeetingTranscripts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
