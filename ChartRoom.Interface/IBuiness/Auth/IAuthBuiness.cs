using ChatRoom.Common.CommonModel;
using ChatRoom.Interface.IBuiness.Base;
using E=ChatRoom.Entity;
using M = ChatRoom.Model;
namespace ChatRoom.Interface.IBuiness.Auth
{
    public interface IAuthBuiness:IAOBusinessBase<M.Auth.Auth>
    {
        M.Auth.Auth CheckAuthForUser(int userId, string authToken, string verifyToken);
        ResultWrapper UpdateAuth(int userId, string authToken, string verifyToken, int expired);
        Model.Auth.Auth CheckAuthForOnlineUser(int userId, string authToken, string verifyToken);
    }
}
