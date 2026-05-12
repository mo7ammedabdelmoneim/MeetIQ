using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetIQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMeetingSummaryColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KeyInsights",
                table: "MeetingSummaries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "MeetingSummaries",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeyInsights",
                table: "MeetingSummaries");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "MeetingSummaries");
        }
    }
}
