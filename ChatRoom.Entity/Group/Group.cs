using ChatRoom.Entity.Base;

namespace ChatRoom.Entity.Group
{
    public class Group:EnitityBase
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Describe { get; set; }
        public string GroupImg { get; set; }
    }
}
