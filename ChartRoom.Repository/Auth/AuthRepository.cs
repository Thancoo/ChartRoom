using System;
using System.Linq;
using ChatRoom.Common;
using ChatRoom.Common.CommonModel;
using ChatRoom.Interface.IRepository.Auth;
using ChatRoom.Repository.Base;
using Dapper;
using E=ChatRoom.Entity;
namespace ChatRoom.Repository.Auth
{
    class AuthRepository:AoBaseRepository<E.Auth.Auth>,IAuthRepository
    {
        public E.Auth.Auth CheckAuthForOnlineUser(int userId, string authToken, string verifyToken)
        {
            var sql ="select * from Auth where UserId=@userId and AuthToken=@authToken and VerifyToken=@verifyToken and GetDBDate() BETWEEN UpdatedOn and ADDDATE(UpdatedOn,INTERVAL Expired MINUTE);";
            try
            {
                using (var conn = this.Connection)
                {
                    return conn.Query<E.Auth.Auth>(sql, new {userId, authToken, verifyToken}).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(GetType(),ex);
                return null;
            }
        }

        public ResultWrapper UpdateAuth(int userId, string authToken, string verifyToken,int expired)
        {
            var sql = "update Auth set AuthToken=@authToken,VerifyToken=@verifyToken,Expired=@expired,UpdatedOn=GetDBDate(),UpdatedBy='System' where Available=1 and UserId=@userId;";
            if (!this.Exists(new Entity.Auth.Auth() {UserId = userId}))
                sql= "insert into Auth(UserId,AuthToken,VerifyToken,Expired,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,Available)values(@userId,@authToken,@verifyToken,@expired,'System',GetDBDate(),'System',GetDBDate(),1);";
            try
            {
                using (var conn = this.Connection)
                {
                    var res= conn.Execute(sql, new {userId, authToken, verifyToken, expired });
                    return new ResultWrapper()
                    {
                        State = res==1,
                        Message = res==1?"更新成功":"更新异常"
                    };
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(GetType(),ex);
                return new ResultWrapper()
                {
                    State = false,
                    Message = "系统错误！",
                    Exception = ex
                }; ;
            }
        }

        public Entity.Auth.Auth CheckAuthForUser(int userId, string authToken, string verifyToken)
        {
            var sql = "select * from Auth where UserId=@userId and AuthToken=@authToken and VerifyToken=@verifyToken;";
            try
            {
                using (var conn = this.Connection)
                {
                    return conn.Query<E.Auth.Auth>(sql, new { userId, authToken, verifyToken }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(GetType(), ex);
                return null;
            }
        }
    }
}
