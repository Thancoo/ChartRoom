using System;
using System.Web;
using System.Web.Http;
using ChatRoom.Common.CommonModel;
using ChatRoom.Common.Utils;
using ChatRoom.Controllers.Base;
using ChatRoom.Filter;
using ChatRoom.Interface.IBuiness.Auth;
using ChatRoom.Interface.IBuiness.Group;
using ChatRoom.Interface.IBuiness.Operation;
using ChatRoom.Interface.IBuiness.User;

namespace ChatRoom.Controllers
{
    public class AuthController: ApiControllerBase
    {
        private readonly IAuthBuiness _authBll;
        private readonly IUserBuiness _userBll;
        private readonly IGroupBuiness _groupBll;
        private readonly IOperationMessageBuiness _goperationmessageBll;

        public AuthController(IAuthBuiness authBll, IUserBuiness userBll, IGroupBuiness groupBll, IOperationMessageBuiness goperationmessageBll)
        {
            this._authBll = authBll;
            this._userBll = userBll;
            this._groupBll = groupBll;
            this._goperationmessageBll = goperationmessageBll;
        }
        [CustomerAllowAnonymous]
        public ResultWrapper AuthConnection()
        {
            if (HttpContext.Current.Request.Cookies[ConfigurationHelper.UserIdName] == null)
            {
                //todo:游客处理程序。注册游客身份
                var user=this._userBll.GenerateVisitorInfo();
                var group_res = this._goperationmessageBll.AddUserToGroupInternal(user.Id,ConfigurationHelper.DefultGroupId);
                if(!group_res.State)
                    return new ResultWrapper() {State = false,Message = "系统在分组阶段出现异常！"};
                var auth_gn=this._userBll.GenerateAuth(user.Id, user.Name, user.Password);
                HttpContext.Current.Response.Cookies.Add(new HttpCookie(ConfigurationHelper.AuthTokenName, auth_gn.AuthToken) { Expires = DateTimeHelper.LocalDateTime.AddDays(ConfigurationHelper.AuthTokenExpiredDays) });
                HttpContext.Current.Response.Cookies.Add(new HttpCookie(ConfigurationHelper.VerifyTokenName, auth_gn.VerifyToken) { Expires = DateTimeHelper.LocalDateTime.AddDays(ConfigurationHelper.AuthTokenExpiredDays) });
                HttpContext.Current.Response.Cookies.Add(new HttpCookie(ConfigurationHelper.UserIdName, auth_gn.UserId.ToString()) { Expires = DateTimeHelper.LocalDateTime.AddYears(100) });
                var res_ua=this._authBll.UpdateAuth(user.Id, auth_gn.AuthToken, auth_gn.VerifyToken, auth_gn.Expired.Value);
                if(res_ua.State)
                    return new ResultWrapper()
                    {
                        State = true,
                        Message = "游客身份注册成功！",
                        StateCode = 1000,
                        Data = user.Id.ToString()
                    };
                return new ResultWrapper()
                {
                    State = false,
                    Message = "游客身份注册失败！",
                    StateCode = -1100
                };
            }
            var userId = Convert.ToInt32(HttpContext.Current.Request.Cookies[ConfigurationHelper.UserIdName].Value);
            var userInfo = this._userBll.GetDataById(userId);
            if (HttpContext.Current.Request.Cookies[ConfigurationHelper.AuthTokenName] == null)
            {
                //游客身份过期，直接重新生成认证。
                if (userInfo.UserType == ConfigurationHelper.UserTypeVisitor)
                {
                    var user = this._userBll.GenerateVisitorInfo();
                    var auth_gn = this._userBll.GenerateAuth(user.Id, user.Name, user.Password);
                    var group_res = this._goperationmessageBll.AddUserToGroupInternal(user.Id, ConfigurationHelper.DefultGroupId);
                    if (!group_res.State)
                        return new ResultWrapper() { State = false, Message = "系统在分组阶段出现异常！" };
                    HttpContext.Current.Response.Cookies.Add(
                        new HttpCookie(ConfigurationHelper.AuthTokenName, auth_gn.AuthToken)
                        {
                            Expires = DateTimeHelper.LocalDateTime.AddDays(ConfigurationHelper.AuthTokenExpiredDays)
                        });
                    HttpContext.Current.Response.Cookies.Add(
                        new HttpCookie(ConfigurationHelper.VerifyTokenName, auth_gn.VerifyToken)
                        {
                            Expires = DateTimeHelper.LocalDateTime.AddDays(ConfigurationHelper.VerifyExpiredDays)
                        });
                    HttpContext.Current.Response.Cookies.Add(
                        new HttpCookie(ConfigurationHelper.UserIdName, auth_gn.UserId.ToString())
                        {
                            Expires = DateTimeHelper.LocalDateTime.AddYears(100)
                        });
                    var res_ua = this._authBll.UpdateAuth(user.Id, auth_gn.AuthToken, auth_gn.VerifyToken,auth_gn.Expired.Value);
                    if (res_ua.State)
                        return new ResultWrapper()
                        {
                            State = true,
                            Message = "游客身份注册成功！",
                            StateCode = 1000,
                            Data = userId.ToString()
                        };
                    return new ResultWrapper()
                    {
                        State = false,
                        Message = "游客身份注册失败！",
                        StateCode = -1100
                    };
                }
                //todo:登录身份30天过期。要求登录
                return new ResultWrapper()
                {
                    State = true,
                    Message = "登录状态已过期，请重新登录！",
                    StateCode = 1101
                };
            }
            //todo:userId可能为空么？
            var authToken = HttpContext.Current.Request.Cookies[ConfigurationHelper.AuthTokenName].Value;
            var verifyToken = HttpContext.Current.Request.Cookies[ConfigurationHelper.VerifyTokenName].Value;
            var auth=this._authBll.CheckAuthForUser(userId, authToken, verifyToken);
            if (auth?.Id==null)
            {
                //todo:异地登录过，在其他地方把Cookie给刷新了。导致Cookie与本地版本不一致。要求重新登录
                if (HttpContext.Current.Response.Cookies[ConfigurationHelper.AuthTokenName] != null)
                    HttpContext.Current.Response.Cookies[ConfigurationHelper.AuthTokenName].Expires =
                        DateTime.Now.AddHours(-ConfigurationHelper.AuthTokenExpiredDays);
                if (HttpContext.Current.Response.Cookies[ConfigurationHelper.VerifyTokenName] != null)
                    HttpContext.Current.Response.Cookies[ConfigurationHelper.VerifyTokenName].Expires =
                        DateTime.Now.AddHours(-ConfigurationHelper.AuthTokenExpiredDays);
                return new ResultWrapper()
                {
                    State = true,
                    Message = "身份验证失败，请重新登录！",
                    StateCode = 1102
                };
            }
            var totalMin = (DateTimeHelper.LocalDateTime - auth.UpdatedOn.Value).TotalMinutes;
            if (totalMin > 0 && totalMin < auth.Expired)
            {
                //todo:在线中，不用操作。
                return new ResultWrapper()
                {
                    State = true,
                    Message = "在线！",
                    StateCode = 1000,
                    Data = userId.ToString()
                };
            }
            //todo:VerifyToken过期，重新刷新VerifyToken。
            var upauth=this._userBll.UpdateAuth(userId, authToken);
            HttpContext.Current.Response.Cookies.Add(
                new HttpCookie(ConfigurationHelper.VerifyTokenName, upauth)
                {
                    Expires = DateTimeHelper.LocalDateTime.AddDays(ConfigurationHelper.VerifyExpiredDays)
                });
            var res=this._authBll.UpdateAuth(userId, authToken, upauth, ConfigurationHelper.VerifyDbExpired);
            if(!res.State)
                return new ResultWrapper()
                {
                    State = false,
                    Message = "更新VerifyToken失败！",
                    StateCode = -1103
                };
            return new ResultWrapper()
            {
                State = true,
                Message = "登录身份更新成功！",
                StateCode= 1000,
                Data = userId.ToString()
            };
        }
        [HttpPost]
        public ResultWrapper HeartBeatAuth()
        {   
            var userId = Convert.ToInt32(HttpContext.Current.Request.Cookies[ConfigurationHelper.UserIdName].Value);
            var authToken = HttpContext.Current.Request.Cookies[ConfigurationHelper.AuthTokenName].Value;
            var verifyToken = HttpContext.Current.Request.Cookies[ConfigurationHelper.VerifyTokenName].Value;
            var auth = this._authBll.CheckAuthForUser(userId, authToken, verifyToken);
            if (auth?.Id == null)
            {
                //todo:异地登录过，在其他地方把Cookie给刷新了。导致Cookie与本地版本不一致。要求重新登录
                if (HttpContext.Current.Response.Cookies[ConfigurationHelper.AuthTokenName] != null)
                    HttpContext.Current.Response.Cookies[ConfigurationHelper.AuthTokenName].Expires =
                        DateTime.Now.AddHours(-ConfigurationHelper.AuthTokenExpiredDays);
                if (HttpContext.Current.Response.Cookies[ConfigurationHelper.VerifyTokenName] != null)
                    HttpContext.Current.Response.Cookies[ConfigurationHelper.VerifyTokenName].Expires =
                        DateTime.Now.AddHours(-ConfigurationHelper.VerifyExpiredDays);
                return new ResultWrapper()
                {
                    State = false,
                    Message = "身份认证已过期，请重新登录!"
                };
            }
            var totalMin = (DateTimeHelper.LocalDateTime - auth.UpdatedOn.Value).TotalMinutes;
            if (totalMin > 0 && totalMin < auth.Expired*0.7)//留点时间更新
            {
                //todo:在线中，不用操作。
                return new ResultWrapper()
                {
                    State = true,
                    Message = "在线！"
                };
            }
            //todo:VerifyToken过期，重新刷新VerifyToken。
            var upauth = this._userBll.UpdateAuth(userId, authToken);
            HttpContext.Current.Response.Cookies.Set((new HttpCookie(ConfigurationHelper.VerifyTokenName, upauth) { Expires = DateTimeHelper.LocalDateTime.AddDays(ConfigurationHelper.AuthTokenExpiredDays) }));
            var res = this._authBll.UpdateAuth(userId, authToken, upauth, ConfigurationHelper.VerifyDbExpired);
            return new ResultWrapper()
            {
                State = true,
                Message = "在线！"
            };
        }
    }
}