using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ChatRoom.Buiness.Base;
using ChatRoom.Common.Utils;
using ChatRoom.Interface.IBuiness.User;
using ChatRoom.Interface.IRepository.User;
using E = ChatRoom.Entity;
using M = ChatRoom.Model;
namespace ChatRoom.Buiness.User
{
    public class UserBuiness : AOBaseBusiness<M.User.User, E.User.User>, IUserBuiness
    {
        private readonly IUserRepository _userRepository;

        public UserBuiness(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public override M.User.User EntityToModel(E.User.User data)
        {
            return new Model.User.User()
            {
                Id = data.Id,
                ConnectionId = data.ConnectionId,
                Name = data.Name,
                Password = data.Password,
                HeadImage = data.HeadImage,
                UserType = data.UserType,
                CreatedOn = data.CreatedOn,
                CreatedBy = data.CreatedBy,
                UpdatedOn = data.UpdatedOn,
                UpdatedBy = data.UpdatedBy,
                Available = data.Available
            };
        }

        public override E.User.User ModelToEntity(M.User.User data)
        {
            return new Entity.User.User()
            {
                Id = data.Id,
                ConnectionId = data.ConnectionId,
                Name = data.Name,
                Password = data.Password,
                Email = data.Email,
                HeadImage = data.HeadImage,
                UserType = data.UserType,
                CreatedOn = data.CreatedOn,
                CreatedBy = data.CreatedBy,
                UpdatedOn = data.UpdatedOn,
                UpdatedBy = data.UpdatedBy,
                Available = data.Available
            };
        }

        public IEnumerable<Model.User.User> GetUserByName(string name)
        {
            return this._userRepository.GetUserByName(name).Select(EntityToModel);
        }

        public Model.User.User GetUserForLogin(string name, string password)
        {
            return EntityToModel(this._userRepository.GetUserForLogin(name,password));
        }

        public Model.Auth.Auth GenerateAuth(int id, string name, string userPassword)
        {
            var sac= SHA512.Create();
            sac.ComputeHash(Encoding.UTF8.GetBytes((name + userPassword).ToCharArray()));
            var authToken = BitConverter.ToString(sac.Hash).ToLower().Replace("-", "");
            var mdc = MD5.Create();
            mdc.ComputeHash(Encoding.UTF8.GetBytes(authToken + DateTimeHelper.TimeUnixStamp));
            var verifyToken = BitConverter.ToString(mdc.Hash).ToLower().Replace("-","");
            var auth=new Model.Auth.Auth()
            {
                UserId = id,
                AuthToken = authToken,
                VerifyToken = verifyToken,
                Expired = ConfigurationHelper.VerifyDbExpired
            };
            return auth;
        }

        public IEnumerable<Model.User.User> GetAllOnlineUsers(int userId)
        {
            return this._userRepository.GetAllOnlineFirend(userId).Select(EntityToModel);
        }

        public Model.User.User GenerateVisitorInfo()
        {
            Random random = new Random(DateTimeHelper.TimeUnixStamp);
            var rad=random.Next(100, 999);
            var us=new M.User.User()
            {
                Name = "游客_"+DateTimeHelper.TimeUnixStamp+""+ rad,
                Password = "password",
                HeadImage = ConfigurationHelper.DefaultHeadImageUrl,
                UserType = "visitor"
            };
            var dtId=this.Insert(us,null,null,true);
            if(!dtId.State)
                throw new Exception("添加User出现异常：UserBll.GenerateVisitoryInfo");
            us.Id = Convert.ToInt32(dtId.Data);
            return us;
        }

        public string UpdateAuth(int userId, string authToken)
        {
            var mdc = MD5.Create();
            mdc.ComputeHash(Encoding.UTF8.GetBytes(authToken + DateTimeHelper.TimeUnixStamp));
            return BitConverter.ToString(mdc.Hash).ToLower().Replace("-", "");
        }
    }
}
