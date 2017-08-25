using System;
using System.Linq;
using System.Reflection;
using Autofac;
using ChatRoom.Common;
using ChatRoom.Common.Utils;
using ChatRoom.Interface.IBuiness.Auth;
using ChatRoom.Interface.IBuiness.Group;
using ChatRoom.Interface.IBuiness.User;
using ChatRoom.Model.User;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ChatRoom.Filter
{
    public class HubAuthAttribute: AuthorizeAttribute
    {
        private readonly IUserBuiness _userBll;
        private readonly IAuthBuiness _authBll;
        private readonly IGroupBuiness _groupBll;

        public string Deny { get; set; }
        public string Allow { get; set; }

        public HubAuthAttribute()
        {
            this.Deny = "";
            this.Allow = "all";
            this._userBll = ChatRoomEnv.Container.Resolve<IUserBuiness>();
            this._authBll = ChatRoomEnv.Container.Resolve<IAuthBuiness>();
            this._groupBll = ChatRoomEnv.Container.Resolve<IGroupBuiness>();
        }
        //默认AuthorizerConnectionHub返回的是False！！！注意！！如果要重写，次方法必须覆盖！！贱！
        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            if (request.Cookies[ConfigurationHelper.UserIdName] == null)
            {
                //todo:第一次登录
                return false;
            }
            if (request.Cookies[ConfigurationHelper.AuthTokenName] == null)
            {
                //todo:30天过期登录
                return false;
            }
            var userId = Convert.ToInt32(request.Cookies[ConfigurationHelper.UserIdName].Value);
            if (!this._userBll.Exists(new User() { Id = userId }))
            {
                //todo:userId可能为空么？
                return false;
            }
            var authToken = request.Cookies[ConfigurationHelper.AuthTokenName].Value;
            var verifyToken = request.Cookies[ConfigurationHelper.VerifyTokenName].Value;
            if (this._authBll.CheckAuthForUser(userId, authToken, verifyToken) == null)
            {
                //todo:授权信息有问题，可能是异地登录导致的
                return false;
            }
            if (this._authBll.CheckAuthForOnlineUser(userId, authToken, verifyToken) == null)
            {
                //todo:登录身份过期，VerifyToken过期。心跳包有问题
                return false;
            }
            return true;
        }

        public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
        {
            var hubAttr = hubIncomingInvokerContext.Hub.GetType().GetCustomAttribute<CustomerAllowAnonymousAttribute>();
            var hubMethodAttr =hubIncomingInvokerContext.MethodDescriptor.Attributes.FirstOrDefault(pp => pp is CustomerAllowAnonymousAttribute);
            if (hubAttr != null|| hubMethodAttr!=null)
                return true;
            var userId = Convert.ToInt32(hubIncomingInvokerContext.Hub.Context.RequestCookies[ConfigurationHelper.UserIdName].Value);
            var user = this._userBll.Get(new User() { Id = userId }).FirstOrDefault();
            //todo:处理Api权限问题
            var denys = this.Deny.ToLower().Split(',');
            var allows = this.Allow.ToLower().Split(',');
            var usertype = user.UserType.ToLower();
            if (!denys.Contains(usertype) && allows.Contains(usertype) ||
                denys.Contains("all") && allows.Contains(usertype) ||
                !denys.Contains(usertype) && !allows.Contains(usertype) ||
                !denys.Contains(usertype) && !allows.Contains("all"))
            {
                //todo:维护用户与ConnectionId与用户的映射
                if (user.ConnectionId != hubIncomingInvokerContext.Hub.Context.ConnectionId)
                {
                    this._userBll.Update(new User() { Id = user.Id, ConnectionId = hubIncomingInvokerContext.Hub.Context.ConnectionId });
                }
                //将用户分组
                var groups = this._groupBll.GetAllGroups(user.Id);
                groups.AsParallel().ForAll(pp => hubIncomingInvokerContext.Hub.Groups.Add(hubIncomingInvokerContext.Hub.Context.ConnectionId, pp.Id.ToString()));
                //todo:服务通过
                LogHelper.WriteLog(GetType(), "授权成功：" + userId );
                return true;
            }
            return false;
        }
    }
}