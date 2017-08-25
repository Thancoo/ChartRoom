using System;
using Autofac;
using ChatRoom.Common;
using ChatRoom.Common.Utils;
using ChatRoom.Interface.IBuiness.Auth;
using Microsoft.AspNet.SignalR;

namespace ChatRoom.Hubs.Module
{
    public class DbUserIdProvider:IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            //可能是没开启Identity的授权，所以Context.User.Identity.Name一直为空。
            //我自己写了权限认证，所以这部分代码弃用。
            if (request.Cookies[ConfigurationHelper.UserIdName] == null)
            {
                throw new Exception("这个人从哪里来的？肯定Auth有问题！！不然不可能进来的！");
            }
            var userId=Convert.ToInt32(request.Cookies[ConfigurationHelper.UserIdName].Value);
            var authToken = request.Cookies[ConfigurationHelper.AuthTokenName].Value;
            var verifyToken = request.Cookies[ConfigurationHelper.VerifyTokenName].Value;
            var authBll = ChatRoomEnv.Container.Resolve<IAuthBuiness>();
            var res=authBll.CheckAuthForUser(userId,authToken,verifyToken);
            if (res != null)
                return userId.ToString();
            throw new Exception("这个人授权过期了？怎么呼吸包不起作用了哪！");
        }
    }
}