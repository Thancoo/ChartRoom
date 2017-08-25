namespace ChatRoom.Model.Message
{
    public class Message:Base.ModelBase
    {
        public int Id { get; set; }
        public string EventType { get; set; }
        public string MsgType { get; set; }
        public string ContentType { get; set; }
        public string Content { get; set; }
    }
}
