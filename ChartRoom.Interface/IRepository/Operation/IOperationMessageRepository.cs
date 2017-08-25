using ChatRoom.Common.CommonModel;
using ChatRoom.Interface.IRepository.Base;
using E=ChatRoom.Entity;
namespace ChatRoom.Interface.IRepository.Operation
{
    public interface IOperationMessageRepository:IAORepositoryBase<E.Operation.OperationMessage>
    {
        ResultWrapper AddUserToGroupInternal(int userId,int groupId);
        ResultWrapper AddUsersToGroupInternal(int[] userIds,int groupId);
    }
}
