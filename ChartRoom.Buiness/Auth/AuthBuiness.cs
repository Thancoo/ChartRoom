using ChatRoom.Buiness.Base;
using ChatRoom.Common.CommonModel;
using ChatRoom.Interface.IBuiness.Auth;
using ChatRoom.Interface.IRepository.Auth;
using E = ChatRoom.Entity;
using M = ChatRoom.Model;
namespace ChatRoom.Buiness.Auth
{
    class AuthBuiness:AOBaseBusiness<M.Auth.Auth,E.Auth.Auth>,IAuthBuiness
    {
        private readonly IAuthRepository _authRepository;
        public AuthBuiness(IAuthRepository authRepository)
        {
            this._authRepository = authRepository;
        }

        public override Model.Auth.Auth EntityToModel(Entity.Auth.Auth entity)
        {
            return new Model.Auth.Auth()
            {
                Id = entity.Id,
                UserId = entity.UserId,
                AuthToken = entity.AuthToken,
                VerifyToken = entity.VerifyToken,
                CreatedOn = entity.CreatedOn,
                CreatedBy = entity.CreatedBy,
                UpdatedOn = entity.UpdatedOn,
                UpdatedBy = entity.UpdatedBy,
                Available = entity.Available
            };
        }

        public override E.Auth.Auth ModelToEntity(M.Auth.Auth model)
        {
            return new E.Auth.Auth()
            {
                Id = model.Id,
                UserId = model.UserId,
                AuthToken = model.AuthToken,
                VerifyToken = model.VerifyToken,
                CreatedOn = model.CreatedOn,
                CreatedBy = model.CreatedBy,
                UpdatedOn = model.UpdatedOn,
                UpdatedBy = model.UpdatedBy,
                Available = model.Available
            };
        }

        public Model.Auth.Auth CheckAuthForUser(int userId, string authToken, string verifyToken)
        {
            return EntityToModel(this._authRepository.CheckAuthForUser(userId, authToken, verifyToken)??new Entity.Auth.Auth());
        }
        public Model.Auth.Auth CheckAuthForOnlineUser(int userId, string authToken, string verifyToken)
        {
            return EntityToModel(this._authRepository.CheckAuthForOnlineUser(userId, authToken, verifyToken) ?? new Entity.Auth.Auth());
        }
        public ResultWrapper UpdateAuth(int userId, string authToken, string verifyToken, int expired)
        {
            return this._authRepository.UpdateAuth(userId, authToken, verifyToken, expired);
        }
    }
}
