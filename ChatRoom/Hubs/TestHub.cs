using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChatRoom.Filter;
using ChatRoom.Hubs.Base;
using Microsoft.AspNet.SignalR;

namespace ChatRoom.Hubs
{
    public class TestHub: HubBase
    {

        public void Test()
        {
            Clients.All.sendMessageTest("我就是做一个测试");
        }
    }
}