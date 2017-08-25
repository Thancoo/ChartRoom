using System.Collections.Generic;
using ChatRoom.Entity.Group;
using ChatRoom.Interface.IBuiness.Base;
using M = ChatRoom.Model;
namespace ChatRoom.Interface.IBuiness.User
{
    public interface IUserBuiness : IAOBusinessBase<M.User.User>
    {
        IEnumerable<M.User.User> GetUserByName(string name);
        M.User.User GetUserForLogin(string name, string password);
        M.Auth.Auth GenerateAuth(int id, string name, string userPassword);
        IEnumerable<M.User.User> GetAllOnlineUsers(int userId);
        M.User.User GenerateVisitorInfo();
        string UpdateAuth(int userId, string authToken);
    }
}
