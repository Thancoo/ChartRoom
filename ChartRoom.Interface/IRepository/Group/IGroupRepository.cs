using System.Collections.Generic;
using ChatRoom.Common.ResponseModel;
using E=ChatRoom.Entity;
namespace ChatRoom.Interface.IRepository.Group
{
    public interface IGroupRepository
    {
        IEnumerable<E.Group.Group> GetAllGroups(int userId);
    }
}
