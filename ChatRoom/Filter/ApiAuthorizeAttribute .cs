using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Autofac;
using ChatRoom.Common;
using ChatRoom.Common.CommonModel;
using ChatRoom.Common.Utils;
using ChatRoom.Interface.IBuiness.Auth;
using ChatRoom.Interface.IBuiness.User;
using ChatRoom.Model.User;
using Newtonsoft.Json;

namespace ChatRoom.Filter
{
    public class ApiAuthorizeAttribute : AuthorizationFilterAttribute
    {
        private readonly IUserBuiness _userBll;
        private readonly IAuthBuiness _authBll;
        public string Deny { get; set; }
        public string Allow { get; set; }

        public ApiAuthorizeAttribute()
        {
            this.Deny = "";
            this.Allow = "all";
            this._userBll = ChatRoomEnv.Container.Resolve<IUserBuiness>();
            this._authBll = ChatRoomEnv.Container.Resolve<IAuthBuiness>();
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var ctrAttr= actionContext.ControllerContext.Controller.GetType().GetCustomAttribute<CustomerAllowAnonymousAttribute>();
            var actAttr = actionContext.ControllerContext.Controller.GetType().GetMethod(actionContext.ControllerContext.RouteData.Values["action"].ToString())?.GetCustomAttributes<CustomerAllowAnonymousAttribute>();
            if (ctrAttr != null|| actAttr!=null)
                return;
            var context = (HttpContextWrapper)actionContext.Request.Properties["MS_HttpContext"];
            //todo:处理Auth验证的问题
            if (context.Request.Cookies[ConfigurationHelper.UserIdName] == null)
            {
                //todo:第一次登录
                actionContext.Response= actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                actionContext.Response.Content =
                    new StringContent(
                        JsonConvert.SerializeObject(
                            new ResultWrapper()
                            {
                                State = false,
                                Message = "请先登录",
                                StateCode = 2100
                            }));
                return;
            }
            if (context.Request.Cookies[ConfigurationHelper.AuthTokenName] == null)
            {
                //todo:30天过期登录
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                actionContext.Response.Content =
                    new StringContent(
                        JsonConvert.SerializeObject(
                            new ResultWrapper()
                            {
                                State = false,
                                Message = "您的登录状态已过期，请重新登录！",
                                StateCode = 2101
                            }));
                return;
            }
            var userId = Convert.ToInt32(HttpContext.Current.Request.Cookies[ConfigurationHelper.UserIdName].Value);
            if (!this._userBll.Exists(new User() {Id = userId}))
            {
                //todo:userId可能为空么？
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                actionContext.Response.Content =
                    new StringContent(
                        JsonConvert.SerializeObject(
                            new ResultWrapper()
                            {
                                State = false,
                                Message = "无法识别您的信息，请重新登录！",
                                StateCode = 2102
                            }));
                return;
            }
            var authToken = HttpContext.Current.Request.Cookies[ConfigurationHelper.AuthTokenName].Value;
            var verifyToken = HttpContext.Current.Request.Cookies[ConfigurationHelper.VerifyTokenName].Value;
            if (this._authBll.CheckAuthForUser(userId, authToken, verifyToken) == null)
            {
                //todo:授权信息有问题，可能是异地登录导致的
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                actionContext.Response.Content =
                    new StringContent(
                        JsonConvert.SerializeObject(
                            new ResultWrapper()
                            {
                                State = false,
                                Message = "身份验证失败，请重新登录！",
                                StateCode = 2103
                            }));
                return;
            }
            if (this._authBll.CheckAuthForOnlineUser(userId, authToken, verifyToken) == null)
            {
                //todo:登录身份过期，VerifyToken过期。心跳包有问题
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                actionContext.Response.Content =
                    new StringContent(
                        JsonConvert.SerializeObject(
                            new ResultWrapper()
                            {
                                State = false,
                                Message = "心跳包更新失败，请确定网络状态！",
                                StateCode = 2104
                            }));
                return;
            }
            //todo:处理Api权限问题
            var user = this._userBll.Get(new User() { Id = userId }).FirstOrDefault();
            var denys = this.Deny.ToLower().Split(',');
            var allows = this.Allow.ToLower().Split(',');
            var usertype = user.UserType.ToLower();
            if (!denys.Contains(usertype) && allows.Contains(usertype) ||
                denys.Contains("all") && allows.Contains(usertype) ||
                !denys.Contains(usertype) && !allows.Contains(usertype) ||
                !denys.Contains(usertype) && !allows.Contains("all"))
            {
                //todo:服务通过
                LogHelper.WriteLog(GetType(), "授权成功：" + userId + ";url=" +actionContext.Request.RequestUri.OriginalString);
                return;
            }
            //todo:访问被拒绝！
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Content =
                new StringContent(
                    JsonConvert.SerializeObject(
                        new ResultWrapper()
                        {
                            State = false,
                            Message = "您没有权限访问该接口！",
                            StateCode = 2105
                        }));
        }
    }
}