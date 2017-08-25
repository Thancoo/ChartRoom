using System;
using Autofac;
using ChatRoom.Common;
using ChatRoom.Common.Utils;
using ChatRoom.Filter;
using ChatRoom.Interface.IBuiness.Auth;
using ChatRoom.Interface.IBuiness.User;
using ChatRoom.Model;
using Microsoft.AspNet.SignalR;

namespace ChatRoom.Hubs.Base
{
    [HubAuth]
    public abstract class HubBase: Hub
    {
        //渣渣，中间隔了一个类就找不到终点了，傻逼！
        protected UserAuthContxt _userAuthContext;

        public UserAuthContxt UserAuthContxt
        {
            get
            {
                if (_userAuthContext == null)
                    _userAuthContext = GetUserAuthContext();
                return _userAuthContext;
            }
        }
        protected UserAuthContxt GetUserAuthContext()
        {
            if (Context.RequestCookies[ConfigurationHelper.UserIdName] == null)
            {
                throw new Exception("这个人从哪里来的？肯定Auth有问题！！不然不可能进来的！");
            }
            var userId = Convert.ToInt32(Context.RequestCookies[ConfigurationHelper.UserIdName].Value);
            var authToken = Context.RequestCookies[ConfigurationHelper.AuthTokenName].Value;
            var verifyToken = Context.RequestCookies[ConfigurationHelper.VerifyTokenName].Value;
            var authBll = ChatRoomEnv.Container.Resolve<IAuthBuiness>();
            var userBll = ChatRoomEnv.Container.Resolve<IUserBuiness>();
            var res = authBll.CheckAuthForUser(userId, authToken, verifyToken);
            if (res == null)
                throw new NullReferenceException("授权为空，因该被拦截器拦截，怎么可能到这儿？？");
            var user = userBll.GetDataById(userId);
            if (user == null)
                throw new NullReferenceException("授权为空，因该被拦截器拦截，怎么可能到这儿？？");
            UserAuthContxt ua = new UserAuthContxt()
            {
                Auth = res,
                User = user
            };
            return ua;
        }
    }
}