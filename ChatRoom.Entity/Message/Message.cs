using System;
using System.Collections.Generic;
using ChatRoom.Entity.Base;

namespace ChatRoom.Entity.Message
{
    public class Message:EnitityBase
    {
        public int Id { get; set; }
        public string EventType { get; set; }
        public string MsgType { get; set; }
        public string ContentType { get; set; }
        public string Content { get; set; }
    }
}
