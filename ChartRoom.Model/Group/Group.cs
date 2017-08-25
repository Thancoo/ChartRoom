using ChatRoom.Model.Base;

namespace ChatRoom.Model.Group
{
    public class Group:ModelBase
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Describe { get; set; }
        public string GroupImg { get; set; }
    }
}
