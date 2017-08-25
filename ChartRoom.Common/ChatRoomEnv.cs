using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNet.SignalR;

namespace ChatRoom.Common
{
    public static class ChatRoomEnv
    {
        public static IContainer Container { get; set; }
    }
}
