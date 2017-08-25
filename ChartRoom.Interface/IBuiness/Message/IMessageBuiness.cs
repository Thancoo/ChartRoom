using System.Collections.Generic;
using ChatRoom.Common.CommonModel;
using ChatRoom.Common.RequestModel;
using ChatRoom.Common.ResponseModel;
using ChatRoom.Interface.IBuiness.Base;
using M = ChatRoom.Model;
namespace ChatRoom.Interface.IBuiness.Message
{
    public interface IMessageBuiness :IAOBusinessBase<M.Message.Message>
    {
        ResultWrapper AddTextMessage(int userId, int groupid, string message);
        ResultWrapper AddImgMessage(int userId, int groupid, string message);
        ResultWrapper AddTextMessages(int userId, int groupid, List<string> message);
        ResultWrapper AddImgMessages(int userId, int groupid, List<string> message);
        ResultWrapper AddMessages(int userId, int groupid, IEnumerable<M.Message.Message> messages);

        ResultWrapper AddMessage(int userId, int groupId, string eventType, string msgType,string contentType, string message);
        IEnumerable<TransMessageModel> GetMessageDetails(TransMessageModel requestMessageModel);
        IEnumerable<GroupChatContext> GetGroupChatContext(int userId);
    }
}
