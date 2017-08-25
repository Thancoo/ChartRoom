using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Common.RequestModel
{
    public class AddUserToGroupRequest
    {
        public int? UserId { get; set; }
        public int? GroupId { get; set; }
    }
}
