using ChatRoom.Common.CommonModel;
using ChatRoom.Model.Operation;

namespace ChatRoom.Interface.IBuiness.Operation
{
    public interface IOperationMessageBuiness:Base.IAOBusinessBase<OperationMessage>
    {
        ResultWrapper AddUserToGroupInternal(int userId, int groupId);
        ResultWrapper AddUsersToGroupInternal(int[] userIds, int groupId);
    }
}
