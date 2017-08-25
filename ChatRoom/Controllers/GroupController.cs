using System.Linq;
using System.Web.Http;
using ChatRoom.Common.CommonModel;
using ChatRoom.Common.RequestModel;
using ChatRoom.Controllers.Base;
using ChatRoom.Interface.IBuiness.Group;
using ChatRoom.Interface.IBuiness.Operation;
using ChatRoom.Model.Group;

namespace ChatRoom.Controllers
{
    public class GroupController: ApiControllerBase
    {
        private readonly IGroupBuiness _groupBll;
        private readonly IOperationMessageBuiness _operationMessageBll;


        public GroupController(IGroupBuiness groupBll, IOperationMessageBuiness operationMessageBll)
        {
            this._groupBll = groupBll;
            this._operationMessageBll = operationMessageBll;
        }
        [HttpPost]
        public ResultWrapper GetGroupInfoById(int groupId)
        {
            var data=this._groupBll.GetDataById(groupId);
            if (data == null)
            {
                return new ResultWrapper()
                {
                    State = false,
                    Message = "该分组不存在！"
                };
            }
            return new ResultWrapper()
            {
                State = true,
                Message = "分组获取成功！",
                Data = data
            };
        }
        [HttpPost]
        public ResultWrapper Get(Group group)
        {
            var data = this._groupBll.Get(group);
            if (data == null)
            {
                return new ResultWrapper(false, "该分组不存在！");
            }
            return new ResultWrapper()
            {
                State = true,
                Message = "分组获取成功！",
                Data = data
            };
        }
        [HttpPost]
        public ResultWrapper GetAllGroups()
        {
            var res=this._groupBll.GetAllGroups(UserAuthContxt.User.Id);
            if(res!=null&&res.Any())
                return new ResultWrapper(true,"分组数据获取成功")
                {
                    Data = res
                };
            return new ResultWrapper(false,"分组数据不存在！");
        }

        [HttpPost]
        public ResultWrapper AddUserToGroup(AddUserToGroupRequest req)
        {
            if(!req.GroupId.HasValue)
                return new ResultWrapper(false,"Group参数缺失。");
            if (!req.UserId.HasValue)
                return new ResultWrapper(false, "UserId参数缺失。" );
            var group= this._groupBll.GetDataById(req.GroupId.Value);
            if(group==null)
                return new ResultWrapper(false,"分组不存在！");
            var gpRep = this._operationMessageBll.AddUserToGroupInternal(req.UserId.Value, req.GroupId.Value);
            if(gpRep.State)
                return new ResultWrapper(true,"分组成功！");
            return new ResultWrapper(false,"分组失败！");
        }
    }
}