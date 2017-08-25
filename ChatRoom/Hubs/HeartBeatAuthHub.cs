using System;
using System.Web;
using Autofac;
using ChatRoom.Common;
using ChatRoom.Common.Utils;
using ChatRoom.Interface.IBuiness.Auth;
using ChatRoom.Interface.IBuiness.User;

namespace ChatRoom.Hubs
{
    public class HeartBeatAuthHub:Base.HubBase
    {
        private readonly IAuthBuiness _authBll;
        private readonly IUserBuiness _userBll;
        public HeartBeatAuthHub(IAuthBuiness authBll, IUserBuiness userBll)
        {
            this._authBll = authBll;
            this._userBll = userBll;
        }
        public void HeartBeatAuth()
        {
            var userId = Convert.ToInt32(Context.Request.Cookies[ConfigurationHelper.UserIdName].Value);
            var authToken = Context.Request.Cookies[ConfigurationHelper.AuthTokenName].Value;
            var verifyToken = Context.Request.Cookies[ConfigurationHelper.VerifyTokenName].Value;
            var auth = this._authBll.CheckAuthForUser(userId, authToken, verifyToken);
            if (auth == null)
            {
                //todo:异地登录过，在其他地方把Cookie给刷新了。导致Cookie与本地版本不一致。要求重新登录
                if (HttpContext.Current.Response.Cookies[ConfigurationHelper.AuthTokenName] != null)
                    HttpContext.Current.Response.Cookies[ConfigurationHelper.AuthTokenName].Expires =
                        DateTime.Now.AddHours(-ConfigurationHelper.AuthTokenExpiredDays);
                if (HttpContext.Current.Response.Cookies[ConfigurationHelper.VerifyTokenName] != null)
                    HttpContext.Current.Response.Cookies[ConfigurationHelper.VerifyTokenName].Expires =
                        DateTime.Now.AddHours(-ConfigurationHelper.VerifyExpiredDays);
                Clients.Caller.Relogin();
                return;
            }
            var totalMin = (DateTimeHelper.LocalDateTime - auth.UpdatedOn.Value).TotalMinutes;
            if (totalMin > 0 && totalMin < auth.Expired)
            {
                //todo:在线中，不用操作。
            }
            //todo:VerifyToken过期，重新刷新VerifyToken。
            var upauth = this._userBll.UpdateAuth(userId, authToken);
            HttpContext.Current.Response.Cookies.Set((new HttpCookie(ConfigurationHelper.VerifyTokenName, upauth) { Expires = DateTimeHelper.LocalDateTime.AddDays(ConfigurationHelper.AuthTokenExpiredDays) }));
            var res = this._authBll.UpdateAuth(userId, authToken, upauth, ConfigurationHelper.VerifyDbExpired);
        }
    }
}