using ChatRoom.Common.CommonModel;
using ChatRoom.Interface.IBuiness.Operation;
using ChatRoom.Interface.IRepository.Operation;
using ChatRoom.Model.Operation;

namespace ChatRoom.Buiness.Operation
{
    using E = Entity;
    using M = Model;
    public class OperationMessageBuiness:Base.AOBaseBusiness<M.Operation.OperationMessage,E.Operation.OperationMessage>,IOperationMessageBuiness
    {
        private readonly IOperationMessageRepository _operationMessageRepository;

        public OperationMessageBuiness(IOperationMessageRepository operationMessageRepository)
        {
            this._operationMessageRepository = operationMessageRepository;
        }

        public override OperationMessage EntityToModel(Entity.Operation.OperationMessage data)
        {
            return new OperationMessage()
            {
                Id = data.Id,
                OperationState = data.OperationState,
                OperatorId = data.OperatorId,
                OperationTargetId = data.OperationTargetId,
                OperationAttach = data.OperationAttach,
                CreatedBy = data.CreatedBy,
                CreatedOn = data.CreatedOn,
                UpdatedBy = data.UpdatedBy,
                UpdatedOn = data.UpdatedOn,
                Available = data.Available
            };
        }

        public override E.Operation.OperationMessage ModelToEntity(OperationMessage data)
        {
            return new E.Operation.OperationMessage()
            {
                Id = data.Id,
                OperationState = data.OperationState,
                OperatorId = data.OperatorId,
                OperationTargetId = data.OperationTargetId,
                OperationAttach = data.OperationAttach,
                CreatedBy = data.CreatedBy,
                CreatedOn = data.CreatedOn,
                UpdatedBy = data.UpdatedBy,
                UpdatedOn = data.UpdatedOn,
                Available = data.Available
            };
        }

        public ResultWrapper AddUserToGroupInternal(int userId, int groupId)
        {
            return this._operationMessageRepository.AddUserToGroupInternal(userId, groupId);
        }

        public ResultWrapper AddUsersToGroupInternal(int[] userIds, int groupId)
        {
            return this._operationMessageRepository.AddUsersToGroupInternal(userIds, groupId);
        }
    }
}
