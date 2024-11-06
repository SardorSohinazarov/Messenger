using Messenger.Domain.Enums;

namespace Messenger.Application.Models.DataTransferObjects.Filters
{
    public class ChatFilter
    {
        public string? UserName { get; set; }
        public EChatType? ChatType { get; set; }
    }
}
