using ChatRoom.Model.Base;

namespace ChatRoom.Model.User
{
    public class User:ModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string Email { get; set; }
        public string HeadImage { get; set; }
        public string UserType { get; set; }
        public string Password { get; set; }
        public string ConnectionId { get; set; }
    }
}
