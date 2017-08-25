using System.Collections.Generic;
using ChatRoom.Common.ResponseModel;
using ChatRoom.Interface.IRepository.Group;
using Dapper;
using E=ChatRoom.Entity;
namespace ChatRoom.Repository.Group
{
    public class GroupRepository:Base.AoBaseRepository<E.Group.Group>,IGroupRepository
    {
        public IEnumerable<Entity.Group.Group> GetAllGroups(int userId)
        {
            var sql = @"select c.* from OperationMessage a 
inner join OperationType b on a.OperationTypeid=b.Id
inner join `Group` c on a.OperationTargetId=c.Id
where b.`Name` in('JoinGrup') and a.OperatorId=@userId";
            using (var conn = this.Connection)
            {
                return conn.Query<E.Group.Group>(sql, new {userId});
            }
        }
    }
}

