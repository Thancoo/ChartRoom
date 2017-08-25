using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ChatRoom.Common.CommonModel;
using ChatRoom.Common.Utils;
using ChatRoom.Controllers.Base;
using ChatRoom.Filter;
using ChatRoom.Interface.IBuiness.Auth;
using ChatRoom.Interface.IBuiness.Group;
using ChatRoom.Interface.IBuiness.Operation;
using ChatRoom.Interface.IBuiness.User;
using ChatRoom.Model.User;

namespace ChatRoom.Controllers
{
    public class UserController : ApiControllerBase
    {
        private readonly IUserBuiness _userBll;
        private readonly IAuthBuiness _authBll;
        private readonly IOperationMessageBuiness _operationMessageBll;

        public UserController(IUserBuiness userBll, IAuthBuiness authBll, IGroupBuiness groupBll, IOperationMessageBuiness operationMessageBll)
        {
            this._userBll = userBll;
            this._authBll = authBll;
            this._operationMessageBll = operationMessageBll;
        }
        [HttpPost]
        [CustomerAllowAnonymous]
        public ResultWrapper Register(UserModel userModel)
        {
            if(string.IsNullOrEmpty(userModel.UserName))
                return new ResultWrapper()
                {
                    StateCode = -1201,
                    Message = "用户名称不可为空！"
                };
            if(this._userBll.GetUserByName(userModel.UserName).Any())
                return new ResultWrapper()
                {
                    StateCode = -1234,
                    Message = "该用户名已被占用，请重新输入！"
                };
            if (string.IsNullOrEmpty(userModel.Email))
                return new ResultWrapper()
                {
                    StateCode = -1201,
                    Message = "Email不可为空！"
                };
            if (string.IsNullOrEmpty(userModel.Password))
                return new ResultWrapper()
                {
                    StateCode = -1201,
                    Message = "密码不可为空！"
                };
            if (string.IsNullOrEmpty(userModel.ConfirmPassword))
                return new ResultWrapper()
                {
                    StateCode = -1201,
                    Message = "确认密码不可为空！"
                };
            if (userModel.ConfirmPassword!=userModel.Password)
                return new ResultWrapper()
                {
                    StateCode = -1201,
                    Message = "密码不一致！"
                };
            var us=new User()
            {
                Name = userModel.UserName,
                HeadImage = userModel.HeardImage??ConfigurationHelper.DefaultHeadImageUrl,
                Sex = userModel.Sex??"男",
                UserType = ConfigurationHelper.UserTypeGenericVip,
                Password=userModel.Password
            };
            var res = this._userBll.Insert(us,null,null,true);
            if(!res.State)
                return new ResultWrapper()
                {
                    State = false,
                    StateCode = -1102,
                    Message = "账号创建失败！"
                
                };
            var groupResDf = this._operationMessageBll.AddUserToGroupInternal(Convert.ToInt32(res.Data),ConfigurationHelper.DefultGroupId);
            if (!groupResDf.State)
                return new ResultWrapper() { State = false, Message = "系统在分组阶段出现异常！" };
            var groupResGs = this._operationMessageBll.AddUserToGroupInternal(Convert.ToInt32(res.Data), ConfigurationHelper.GenericGroupId);
            if (!groupResGs.State)
                return new ResultWrapper() { State = false, Message = "系统在分组阶段出现异常！" };
            return new ResultWrapper()
            {
                StateCode =0,
                Message = "创建成功！",
                Data = res.Data
            };
        }
        [HttpPost]
        [CustomerAllowAnonymous]
        public ResultWrapper Login(UserModel user)
        {
            if(string.IsNullOrEmpty(user?.UserName))
                return new ResultWrapper()
                {
                    State = false,
                    StateCode = -2212,
                    Message = "用户名不可为空！"
                };
            if (string.IsNullOrEmpty(user?.Password))
                return new ResultWrapper()
                {
                    State = false,
                    StateCode = -2212,
                    Message = "密码不可为空！"
                };
            var user_db=this._userBll.GetUserByName(user.UserName).FirstOrDefault();
            if (user_db==null)
            {
                return new ResultWrapper()
                {
                    State = false,
                    StateCode = -1120,
                    Message = "当前账号不存在！",
                };
            }
            if(user_db.Password!=user.Password)
                return new ResultWrapper()
                {
                    State = false,
                    StateCode = -1241,
                    Message = "密码错误！"
                };
            Model.Auth.Auth auth= this._userBll.GenerateAuth(user_db.Id, user_db.Name, user.Password);
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(ConfigurationHelper.AuthTokenName,auth.AuthToken) {Expires = DateTimeHelper.LocalDateTime.AddDays(ConfigurationHelper.AuthTokenExpiredDays)});
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(ConfigurationHelper.VerifyTokenName, auth.VerifyToken) {Expires =DateTimeHelper.LocalDateTime.AddDays(ConfigurationHelper.AuthTokenExpiredDays) });
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(ConfigurationHelper.UserIdName, auth.UserId.ToString()) { Expires = DateTimeHelper.LocalDateTime.AddYears(100) });//永久有效
            this._authBll.UpdateAuth(user_db.Id, auth.AuthToken, auth.VerifyToken, auth.Expired.Value);
            return new ResultWrapper()
            {
                State = true,
                StateCode = 0,
                Message = "登录成功！",
                Data = user_db.Id.ToString()
            };
        }
        [HttpPost]
        public ResultWrapper LoginOut()
        {
            if (HttpContext.Current.Response.Cookies[ConfigurationHelper.AuthTokenName] != null)
                HttpContext.Current.Response.Cookies[ConfigurationHelper.AuthTokenName].Expires =
                    DateTime.Now.AddHours(-ConfigurationHelper.AuthTokenExpiredDays);
            if (HttpContext.Current.Response.Cookies[ConfigurationHelper.VerifyTokenName] != null)
                HttpContext.Current.Response.Cookies[ConfigurationHelper.VerifyTokenName].Expires =
                    DateTime.Now.AddHours(-ConfigurationHelper.AuthTokenExpiredDays);
            return new ResultWrapper()
            {
                State = true,
                Message = "退出登录成功！"
            };
        }
        [HttpPost]
        public UserDetail GetUserDetailInfo(int? userId)
        {
            var user = this._userBll.GetDataById(userId??UserAuthContxt.User.Id);
            return new UserDetail()
            {
                Id=user?.Id,
                UserName = user?.Name,
                Email = user?.Email,
                HeadImage = user?.HeadImage,
                UserType = user?.UserType,
                Sex = user?.Sex
            };
        }
        [HttpPost]
        public ResultWrapper GetAllOnlineFirends()
        {
            var res= this._userBll.GetAllOnlineUsers(UserAuthContxt.User.Id).Select(ss=>new UserDetail()
            {
                Id = ss.Id,
                UserName = ss.Name,
                HeadImage = ss.HeadImage,
                UserType=ss.UserType,
                Email = ss.Email,
                Sex = ss.Sex,
                UserState=true
            }).ToList();
            return new ResultWrapper(true,"在线朋友列表获取成功！")
            {
                Data = res
            };
        }
    }
}