using System;
using System.Linq;
using ChatRoom.Common;
using ChatRoom.Common.CommonModel;
using ChatRoom.Interface.IRepository.Operation;
using ChatRoom.Repository.Base;
using Dapper;
using E=ChatRoom.Entity;
namespace ChatRoom.Repository.Operation
{
    public class OperationMessageRepository:AoBaseRepository<E.Operation.OperationMessage>,IOperationMessageRepository
    {
        public ResultWrapper AddUserToGroupInternal(int userId, int groupId)
        {   
            try
            {
                using (var conn = this.Connection)
                {
                    var sql = @"insert into OperationMessage(OperationTypeId,OperatorId,OperationTargetId,OperationState,OperationAttach,Createdby,CreatedOn,UpdatedBy,UpdatedOn,Available)
select Id,@userId,@groupId,1,'系统加群，跨过验证。','yk',GetDBDate(),'yk',GetDBDate(),1 from OperationType where `name`='JoinGrup'";
                    var res = conn.Execute(sql, new {userId, groupId});
                    if (res == 1)
                        return new ResultWrapper()
                        {
                            State = true,
                            Message = "添加分组成功！"
                        };
                    return new ResultWrapper()
                    {
                        State = false,
                        Message = "添加分组异常！"
                    };
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(GetType(),ex);
                return new ResultWrapper()
                {
                    State = false,
                    Message = "添加分组异常！",
                    Exception = ex
                };
            }
        }

        public ResultWrapper AddUsersToGroupInternal(int[] userIds, int groupId)
        {
            var res = userIds.Select(pp => AddUserToGroupInternal(pp, groupId)).All(oo => oo.State);
            return new ResultWrapper()
            {
                State = res,
                Message = res?"批量分组成功": "批量分组失败！",
                Exception = new Exception("详情请查看以上分组失败Log！！")
            };
        }
    }
}
