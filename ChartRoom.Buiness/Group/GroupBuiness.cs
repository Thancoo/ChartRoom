using System.Collections.Generic;
using System.Linq;
using ChatRoom.Buiness.Base;
using ChatRoom.Common.ResponseModel;
using ChatRoom.Interface.IBuiness.Group;
using ChatRoom.Interface.IRepository.Group;
using ChatRoom.Model.Group;
using E = ChatRoom.Entity;
using M = ChatRoom.Model;
namespace ChatRoom.Buiness.Group
{
    public class GroupBuiness : AOBaseBusiness<M.Group.Group, E.Group.Group>, IGroupBuiness
    {
        private readonly IGroupRepository _groupRepository;

        public GroupBuiness(IGroupRepository groupRepository)
        {
            this._groupRepository = groupRepository;
        }

        public override M.Group.Group EntityToModel(E.Group.Group data)
        {
            if (data == null)
                return null;
            return new M.Group.Group()
            {
                Id = data.Id,
                Name = data.Name,
                Describe = data.Describe,
                GroupImg = data.GroupImg,
                CreatedOn = data.CreatedOn,
                CreatedBy = data.CreatedBy,
                UpdatedOn = data.UpdatedOn,
                UpdatedBy = data.UpdatedBy,
                Available = data.Available
            };
        }
        public override Entity.Group.Group ModelToEntity(Model.Group.Group data)
        {
            if (data == null)
                return null;
            return new E.Group.Group()
            {
                Id = data.Id,
                Name = data.Name,
                Describe = data.Describe,
                GroupImg=data.GroupImg,
                CreatedOn = data.CreatedOn,
                CreatedBy = data.CreatedBy,
                UpdatedOn = data.UpdatedOn,
                UpdatedBy = data.UpdatedBy,
                Available = data.Available
            };
        }

        public UserGroupDetail EntityToModel(E.Group.UserGroupDetail entity)
        {
            return new UserGroupDetail()
            {
                UserId = entity.UserId,
                UserName = entity.UserName,
                GroupId = entity.GroupId,
                GroupName = entity.GroupName
            };
        }


        public IEnumerable<Model.Group.Group> GetAllGroups(int userId)
        {
            return this._groupRepository.GetAllGroups(userId).Select(EntityToModel);
        }
    }
}
