using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChatRoom.Entity.Base;

namespace ChatRoom.Entity.Auth
{
    public class Auth:EnitityBase
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public string ConnectionId { get; set; }
        public string AuthToken { get; set; }
        public string VerifyToken { get; set; }
        public int? Expired { get; set; }
    }
}
