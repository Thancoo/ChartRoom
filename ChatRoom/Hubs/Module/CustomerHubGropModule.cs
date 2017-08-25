using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using ChatRoom.Common;
using ChatRoom.Common.Utils;
using ChatRoom.Interface.IBuiness.Group;
using ChatRoom.Interface.IBuiness.User;
using ChatRoom.Model.User;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ChatRoom.Hubs.Module
{
    public class CustomerHubGropModule: HubPipelineModule
    {
        private readonly IGroupBuiness _groupBll;
        private readonly IUserBuiness _userBll;

        public CustomerHubGropModule()
        {
            this._groupBll = ChatRoomEnv.Container.Resolve<IGroupBuiness>();
            this._userBll = ChatRoomEnv.Container.Resolve<IUserBuiness>();
        }

        public override Func<HubDescriptor, IRequest, IList<string>, IList<string>> BuildRejoiningGroups(Func<HubDescriptor, IRequest, IList<string>, IList<string>> rejoiningGroups)
        {
            rejoiningGroups = (hub, req, l) =>
            {
                var userid=Convert.ToInt32(req.Cookies[ConfigurationHelper.UserIdName].Value);
                var gups=this._groupBll.GetAllGroups(userid);
                return gups.Select(pp => pp.Name).ToArray();
            };
            return rejoiningGroups;
        }

        protected override bool OnBeforeConnect(IHub hub)
        {
            //todo:通过授权，即将调用Hub前的事件，可以用来用户与ConnectionId的映射
            var userid = Convert.ToInt32(hub.Context.RequestCookies[ConfigurationHelper.UserIdName].Value);
            var res=this._userBll.Update(new User() {Id = userid, ConnectionId = hub.Context.ConnectionId});
            if(res.State)
                return true;
            throw new Exception("用户永久映射失败！");
        }

        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            LogHelper.WriteLog(GetType(),exceptionContext.Error);
            base.OnIncomingError(exceptionContext, invokerContext);
        }
    }
}