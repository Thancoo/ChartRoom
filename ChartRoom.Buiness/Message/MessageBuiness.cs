using System;
using System.Collections.Generic;
using System.Linq;
using ChatRoom.Buiness.Base;
using ChatRoom.Common.CommonModel;
using ChatRoom.Common.RequestModel;
using ChatRoom.Common.ResponseModel;
using ChatRoom.Interface.IBuiness.Message;
using ChatRoom.Interface.IRepository.Message;
using E = ChatRoom.Entity;
using M = ChatRoom.Model;
namespace ChatRoom.Buiness.Message
{
    public class MessageBuiness:AOBaseBusiness<M.Message.Message,E.Message.Message>,IMessageBuiness
    {
        private readonly IMessageRepository _messageRepository;

        public MessageBuiness(IMessageRepository messageRepository)
        {
            this._messageRepository = messageRepository;
        }

        public override Model.Message.Message EntityToModel(Entity.Message.Message data)
        {
            return new Model.Message.Message()
            {
                Id = data.Id,
                EventType = data.EventType,
                MsgType = data.MsgType,
                ContentType=data.ContentType,
                Content = data.Content,
                CreatedOn = data.CreatedOn,
                UpdatedOn = data.UpdatedOn,
                CreatedBy = data.CreatedBy,
                UpdatedBy = data.UpdatedBy,
                Available = data.Available
            };
        }

        public override Entity.Message.Message ModelToEntity(Model.Message.Message data)
        {
            return new Entity.Message.Message()
            {
                Id = data.Id,
                EventType = data.EventType,
                MsgType = data.MsgType,
                ContentType = data.ContentType,
                Content = data.Content,
                CreatedOn = data.CreatedOn,
                UpdatedOn = data.UpdatedOn,
                CreatedBy = data.CreatedBy,
                UpdatedBy = data.UpdatedBy,
                Available = data.Available
            };
        }

        public ResultWrapper AddTextMessage(int userId, int groupid, string message)
        {
            return this._messageRepository.AddTextMessage(userId, groupid, message);
        }

        public ResultWrapper AddImgMessage(int userId, int groupid, string message)
        {
            return this._messageRepository.AddImgMessage(userId, groupid, message);
        }

        public ResultWrapper AddTextMessages(int userId, int groupid, List<string> message)
        {
            return this._messageRepository.AddTextMessages(userId, groupid, message);
        }

        public ResultWrapper AddImgMessages(int userId, int groupid, List<string> message)
        {
            return this._messageRepository.AddImgMessages(userId, groupid, message);
        }

        public ResultWrapper AddMessages(int userId, int groupid, IEnumerable<Model.Message.Message> messages)
        {
            return this._messageRepository.AddMessages(userId, groupid, messages.Select(ModelToEntity));
        }

        public ResultWrapper AddMessage(int userId, int groupId, string eventType, string msgType,string contentType, string message)
        {
            return this._messageRepository.AddMessage(userId, groupId, eventType, msgType,contentType, message);
        }

        public IEnumerable<TransMessageModel> GetMessageDetails(TransMessageModel requestMessageModel)
        {
            return this._messageRepository.GetMessageDetails(requestMessageModel);
        }

        public IEnumerable<GroupChatContext> GetGroupChatContext(int userId)
        {
            return this._messageRepository.GetGroupChatContext(userId);
        }
    }
}
