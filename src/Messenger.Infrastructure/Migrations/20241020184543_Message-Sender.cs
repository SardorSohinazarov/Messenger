using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messenger.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MessageSender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_SenderChatId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SenderChatId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SenderChatId",
                table: "Messages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SenderChatId",
                table: "Messages",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderChatId",
                table: "Messages",
                column: "SenderChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_SenderChatId",
                table: "Messages",
                column: "SenderChatId",
                principalTable: "Chats",
                principalColumn: "Id");
        }
    }
}
