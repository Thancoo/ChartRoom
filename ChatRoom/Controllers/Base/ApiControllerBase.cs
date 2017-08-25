using System;
using System.Web;
using System.Web.Http;
using Autofac;
using ChatRoom.Common;
using ChatRoom.Common.Utils;
using ChatRoom.Filter;
using ChatRoom.Interface.IBuiness.Auth;
using ChatRoom.Interface.IBuiness.User;
using ChatRoom.Model;

namespace ChatRoom.Controllers.Base
{
    [ApiAuthorize]
    public abstract class ApiControllerBase:ApiController
    {
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

        //没有通过验证的对外接口调用此方法，有小几率抛异常。
        protected UserAuthContxt GetUserAuthContext()
        {
            if (HttpContext.Current.Request.Cookies[ConfigurationHelper.UserIdName] == null)
            {
                throw new Exception("这个人从哪里来的？肯定Auth有问题！！不然不可能进来的！");
            }
            var userId = Convert.ToInt32(HttpContext.Current.Request.Cookies[ConfigurationHelper.UserIdName].Value);
            var authToken = HttpContext.Current.Request.Cookies[ConfigurationHelper.AuthTokenName].Value;
            var verifyToken = HttpContext.Current.Request.Cookies[ConfigurationHelper.VerifyTokenName].Value;
            var authBll = ChatRoomEnv.Container.Resolve<IAuthBuiness>();
            var userBll = ChatRoomEnv.Container.Resolve<IUserBuiness>();
            var res = authBll.CheckAuthForUser(userId, authToken, verifyToken);
            if (res == null)
                throw new NullReferenceException("授权为空，因该被拦截器拦截，怎么可能到这儿？？"); 
            var user=userBll.GetDataById(userId);
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