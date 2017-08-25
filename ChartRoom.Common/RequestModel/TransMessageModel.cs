using System;
using ChatRoom.Common.Utils;

namespace ChatRoom.Common.RequestModel
{
    [Serializable]
    public class TransMessageModel
    {
        
        public int? RelayFromId { get; set; }
        public int? RelayToId { get; set; }
        public string EventType { get; set; }
        public string MsgType { get; set; }
        public string ContentType { get; set; }
        public string Content { get; set; }
        private int? _createdOn;
        public int? CreatedOn {
            get{return this._createdOn ?? DateTimeHelper.TimeUnixStamp;}
            set { this._createdOn=value?? DateTimeHelper.TimeUnixStamp; }
        }
    }
}
