using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Model.User
{
    public class UserDetail
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public string HeadImage { get; set; }
        public int? Id { get; set; }
        public string UserType { get; set; }
        public bool UserState { get; set; }
    }
}
