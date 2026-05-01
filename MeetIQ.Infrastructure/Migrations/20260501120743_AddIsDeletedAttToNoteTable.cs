using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetIQ.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedAttToNoteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Notes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Notes");
        }
    }
}
