using Messenger.Application.Models.Pagination.Cursor;

namespace Messenger.Application.Models.DataTransferObjects.Messages
{
    public class MessagesPaginationSelectingByChatDto : CursorPaginationParam
    {
        public int ChatId { get; set; }
    }
}
