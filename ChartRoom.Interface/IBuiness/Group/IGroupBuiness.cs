using System.Collections.Generic;
using ChatRoom.Common.CommonModel;
using ChatRoom.Common.ResponseModel;
using ChatRoom.Interface.IBuiness.Base;
using ChatRoom.Model.Group;
using M = ChatRoom.Model;
namespace ChatRoom.Interface.IBuiness.Group
{
    public interface IGroupBuiness : IAOBusinessBase<M.Group.Group>
    {
        IEnumerable<M.Group.Group> GetAllGroups(int userId);
    }
}
