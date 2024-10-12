using Messenger.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Application.DataTransferObjects.Filters
{
    public class ChatFilter
    {
        public long? UserId { get; set; }
        public string? UserName { get; set; }
        public EChatType ChatType { get; set; }
    }
}
