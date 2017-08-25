using System;
using System.Collections.Generic;
using System.Linq;
using ChatRoom.Common;
using ChatRoom.Common.CommonModel;
using ChatRoom.Common.RequestModel;
using ChatRoom.Common.ResponseModel;
using ChatRoom.Common.Utils;
using ChatRoom.Interface.IRepository.Message;
using ChatRoom.Repository.Base;
using Dapper;
using E=ChatRoom.Entity;

namespace ChatRoom.Repository.Message
{
    public class MessageRepository : AoBaseRepository<Entity.Message.Message>, IMessageRepository
    {
        public ResultWrapper AddTextMessage(int userId,int groupid, string message)
        {
            return AddMessage(userId, ConfigurationHelper.DefultGroupId, "chat", "message","text", message);
        }
        public ResultWrapper AddImgMessage(int userId, int groupid, string message)
        {
            return AddMessage(userId, ConfigurationHelper.DefultGroupId, "chat", "message", "image", message);
        }
        public ResultWrapper AddTextMessages(int userId, int groupid, List<string> message)
        {
            return AddMessages(userId, groupid,message.Select(m=>new E.Message.Message() {EventType = "message",MsgType = "text",Content = m}));
        }
        public ResultWrapper AddImgMessages(int userId, int groupid, List<string> message)
        {
            return AddMessages(userId, groupid, message.Select(m => new E.Message.Message() { EventType = "message", MsgType = "image", Content = m }));
        }
        public ResultWrapper AddMessages(int userId,int groupid,IEnumerable<Entity.Message.Message> messages)
        {
            if (userId == default(int))
                return new ResultWrapper()
                {
                    State = false,
                    Message = "userId不可为空！"
                };
            if(messages!=null&&!messages.Any())
                return new ResultWrapper()
                {
                    State = false,
                    Message = "没有消息要添加！"
                };
            try
            {
                foreach(var itm in messages)
                {
                    AddMessage(userId, groupid, itm.EventType, itm.MsgType, itm.ContentType, itm.Content);
                }
                return new ResultWrapper()
                {
                    State = true,
                    Message = "添加成功！"
                };
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(GetType(), ex);
                return new ResultWrapper()
                {
                    State = false,
                    Message = "添加留言出现异常！",
                    Exception = ex
                };
            }
        }

        public ResultWrapper AddMessage(int relayFromId,int relayToId,string eventType,string msgType,string contentType, string message)
        {
            if(relayFromId == default(int))
                return new ResultWrapper()
                {
                    State = false,
                    Message = "UserId不可为空！"
                };
            if (relayToId == default(int))
                return new ResultWrapper()
                {
                    State = false,
                    Message = "UserId不可为空！"
                };
            if (string.IsNullOrEmpty(message))
                return new ResultWrapper()
                {
                    State = false,
                    Message = "Message不可为空！"
                };
            eventType = eventType ?? "message";
            msgType = msgType ?? "text";
            var sql = "insert into Message(EventType,MsgType,ContentType,Content,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,Available) values(@eventType,@msgType,@contentType,@message,'System',GetDBDate(),'System',GetDBDate(),1);";
            sql += "insert into UserMessageMapping(RelayFromId,RelayToId,MessageId,Createdby,CreatedOn,UpdatedBy,UpdatedOn,Available) values(@relayFromId,@relayToId,@@IDENTITY,'System',GetDBDate(),'System',GetDBDate(),1);";
            try
            {
                using (var conn = this.Connection)
                {
                    var res=conn.Execute(sql, new { relayFromId,relayToId,eventType ,msgType,contentType,message});
                    return new ResultWrapper()
                    {
                        State = res==2,
                        Message = "添加留言成功！"
                    };
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(GetType(),ex);
                return new ResultWrapper()
                {
                    State = false,
                    Message = "添加留言出现异常！",
                    Exception = ex
                };
            }
        }

        public IEnumerable<TransMessageModel> GetMessageDetails(TransMessageModel requestMessageModel)
        {
            if (requestMessageModel == null)
                return new List<TransMessageModel>();
            var sql = @"select a.RelayFromId RelayFromId,a.RelayToId RelayToId,b.EventType,b.MsgType,b.ContentType,b.Content,UNIX_TIMESTAMP(a.CreatedOn) CreatedOn
from UserMessageMapping a inner join Message b on a.MessageId=b.Id
where 1=1";
            if (requestMessageModel.RelayFromId.HasValue)
                sql += " AND a.RelayFromId=@RelayFromId";
            if (requestMessageModel.RelayToId.HasValue)
                sql += " AND a.RelayToId=@RelayToId";
            if (!string.IsNullOrEmpty(requestMessageModel.EventType))
                sql += " AND b.EventType=@EventType";
            if (!string.IsNullOrEmpty(requestMessageModel.MsgType))
                sql += " AND b.MsgType=@MsgType";
            if (!string.IsNullOrEmpty(requestMessageModel.Content))
                sql += " AND b.Content LIKE CONCAT('%',@Content,'%')";
            using (var conn = this.Connection)
            {
                return conn.Query<TransMessageModel>(sql, requestMessageModel);
            }
        }

        public IEnumerable<GroupChatContext> GetGroupChatContext(int userId)
        {
            var sql = @"create temporary table chatCt
select c.Id,c.`Name`,c.GroupImg HeadImg,'' MsgType,null LastChatTime from OperationMessage a 
inner join OperationType b on a.OperationTypeid=b.Id
inner join `Group` c on a.OperationTargetId=c.Id
where b.`Name` in('JoinGrup') and a.OperatorId=@userId;

select a.Id,a.`Name`,a.HeadImg,c.MsgType,UNIX_TIMESTAMP(MAX(b.CreatedOn)) LastChatTime
from chatCt a
left join UserMessageMapping b on b.RelayToId=a.Id and b.RelayFromId=@userId
left join Message c on c.Id=b.MessageId and c.EventType='chat' and c.MsgType='group'
group by a.Id,a.`Name`,a.HeadImg,c.MsgType
order by LastChatTime desc;

drop table chatCt;";
            using (var conn = this.Connection)
            {
                return conn.Query<GroupChatContext>(sql, new { userId });
            }
        }
    }
}
