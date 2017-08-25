using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Model
{
    public class UserAuthContxt
    {
        public Auth.Auth Auth { get; set; }
        public User.User User { get; set; }
    }
}