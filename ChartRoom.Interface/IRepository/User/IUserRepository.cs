using System.Collections.Generic;
using System.Threading.Tasks;
using ChatRoom.Common.CommonModel;
using ChatRoom.Entity.Group;
using ChatRoom.Interface.IRepository.Base;
using E=ChatRoom.Entity;

namespace ChatRoom.Interface.IRepository.User
{
    public interface IUserRepository : IAORepositoryBase<E.User.User>
    {
        IEnumerable<E.User.User> GetUserByName(string name);
        E.User.User GetUserForLogin(string name,string password);
        IEnumerable<E.User.User> GetAllOnlineFirend(int userId);
        IEnumerable<E.User.User> GetAllFirend(int userId);
    }
}
