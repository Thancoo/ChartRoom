using System.Collections.Generic;
using ChatRoom.Common.CommonModel;
using ChatRoom.Common.RequestModel;
using ChatRoom.Common.ResponseModel;
using ChatRoom.Interface.IRepository.Base;
using E=ChatRoom.Entity;

namespace ChatRoom.Interface.IRepository.Message
{
    public interface IMessageRepository : IAORepositoryBase<E.Message.Message>
    {
        ResultWrapper AddTextMessage(int userId, int groupid, string message);
        ResultWrapper AddImgMessage(int userId, int groupid, string message);
        ResultWrapper AddTextMessages(int userId, int groupid, List<string> message);
        ResultWrapper AddImgMessages(int userId, int groupid, List<string> message);
        ResultWrapper AddMessages(int userId, int groupid, IEnumerable<Entity.Message.Message> messages);
        ResultWrapper AddMessage(int userId, int groupId, string eventType, string msgType, string contentType,string message);
        IEnumerable<TransMessageModel> GetMessageDetails(TransMessageModel requestMessageModel);
        IEnumerable<GroupChatContext> GetGroupChatContext(int userId);
    }
}
