using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.SqlClient;
using System.Linq;
using ChatRoom.Entity.Group;
using ChatRoom.Interface.IRepository.User;
using ChatRoom.Repository.Base;
using Dapper;
using E=ChatRoom.Entity;

namespace ChatRoom.Repository.User
{
    public class UserRepository : AoBaseRepository<E.User.User>, IUserRepository
    {
        public IEnumerable<Entity.User.User> GetUserByName(string name)
        {
            return this.Get(new Entity.User.User() {Name = name });
        }

        public Entity.User.User GetUserForLogin(string name, string password)
        {
            return this.Get(new Entity.User.User() { Name = name, Password = password }).FirstOrDefault();
        }
        public IEnumerable<E.User.User> GetAllFirend(int userId)
        {
            var query = @"select c.* from OperationMessage a 
inner join OperationType b on a.OperationTypeid=b.Id
inner join `User` c on a.OperatorId=c.Id or a.OperationTargetid=c.Id
where a.OperatorId=@userId and b.`Name` in('InviteFirend')";
            using (var conn = this.Connection)
            {
                return conn.Query<E.User.User>(query, new { userId });
            }
        }
        public IEnumerable<Entity.User.User> GetAllOnlineFirend(int userId)
        {
            var query = @"select c.* from OperationMessage a 
inner join OperationType b on a.OperationTypeid=b.Id
inner join `User` c on a.OperatorId=c.Id or a.OperationTargetid=c.Id
inner join Auth d on a.OperationTargetid=d.UserId 
where c.`Id`=@userId and b.`Name` in('InviteFirend')
and GetDBDate() BETWEEN d.UpdatedOn and ADDDATE(d.UpdatedOn, INTERVAL d.Expired MINUTE)";
            using (var conn = this.Connection)
            {
                return conn.Query<E.User.User>(query,new {userId});
            }
        }
        public IEnumerable<E.User.User> GetGroupFirend(int groupId)
        {
            using (var conn = this.Connection)
            {
                var sql = @"select c.* from OperationMessage a 
inner join OperationType b on a.OperationTypeid=b.Id
inner join `User` c on a.OperatorId=c.Id
where a.OperationTargetId=@groupId and b.`Name` in('JoinGrup')";
                return conn.Query<E.User.User>(sql, new { groupId });
            }
        }
    }
}
