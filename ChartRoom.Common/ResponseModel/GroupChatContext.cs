using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Common.ResponseModel
{
    public class GroupChatContext
    {
        public int? Id { get; set; }

        public string Name { get; set; }
        public string HeadImg { get; set; }
        public string EventType { get; set; }
        public string MsgType { get; set; }
        public int? LastChatTime { get; set; }
    }
}
