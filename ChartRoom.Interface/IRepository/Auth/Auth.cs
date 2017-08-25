using ChatRoom.Common.CommonModel;
using ChatRoom.Interface.IRepository.Base;
using E=ChatRoom.Entity;
namespace ChatRoom.Interface.IRepository.Auth
{
    public interface IAuthRepository:IAORepositoryBase<E.Auth.Auth>
    {
        E.Auth.Auth CheckAuthForUser(int userId, string authToken, string verifyToken);
        ResultWrapper UpdateAuth(int userId, string authToken, string verifyToken, int expired);
        E.Auth.Auth CheckAuthForOnlineUser(int userId, string authToken, string verifyToken);
    }
}
