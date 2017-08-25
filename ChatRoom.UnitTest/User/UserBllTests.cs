using System;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using ChatRoom.Common;
using ChatRoom.Interface.IBuiness.Auth;
using ChatRoom.Interface.IBuiness.Group;
using ChatRoom.Interface.IBuiness.Message;
using ChatRoom.Repository.User;
using ChatRoom.UnitTest;

namespace ChatRoom.Buiness.User.Tests
{
    [TestClass()]
    public class UserBllTests
    {
        private User.UserBuiness userBll;
        [TestInitialize]
        public void Initial()
        {
            this.userBll = new User.UserBuiness(new UserRepository());
            IcoConfig.Register();
        }
        [TestMethod()]
        public void GetUserByNameTest()
        {
            var userBll = ChatRoomEnv.Container.Resolve<Interface.IBuiness.User.IUserBuiness>();
            var res = userBll.GetUserByName("");
        }

        [TestMethod()]
        public void GenerateAuthTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetUserForLoginTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllOnlineUsersTest()
        {
            Assert.Fail();
        }
        [TestMethod()]
        public void Insert()
        {
            var res = userBll.Insert(new Model.User.User()
            {
                Name = "Thancoo加中文三",
                Email = "ajsofjao@qq.com",
                HeadImage = "asofjaeo",
                Password = "password",
                Sex = "男",
                UserType = "vip"
            },null,null,true);
            Assert.IsTrue(res!=null&&res.State);
            //var us=userBll.GetUserByName("Thancoo").FirstOrDefault();
            //Assert.IsNotNull(us);
            //var groupBll = ChatRoomEnv.Container.Resolve<IGroupBuiness>();
            //var res_groupuser=groupBll.(Convert.ToInt32(res.Data), new ChatRoom.Model.Group.Group() {Name = "默认分组",Describe = "所有新加入的人默认都是这里的人！"});
            //Assert.IsNotNull(res_groupuser);
            //Assert.IsTrue(res_groupuser.State);
            //var auth= userBll.GenerateAuth(Convert.ToInt32(res.Data), "Thancoo", "password");
            //Assert.IsTrue(auth!=null);
            //var authBll = ChatRoomEnv.Container.Resolve<IAuthBuiness>();
            //var re_auth= authBll.Insert(auth);
            //Assert.IsTrue(re_auth.State);
            //var vf=authBll.CheckAuthForUser(Convert.ToInt32(res.Data), auth.AuthToken, auth.VerifyToken);
            //Assert.IsTrue(vf!=null);
            //var messageBll = ChatRoomEnv.Container.Resolve<IMessageBuiness>();
            //var addmessage_state= messageBll.AddTextMessage(Convert.ToInt32(res.Data), 1, "我就做一个测试！");
            //Assert.IsTrue(addmessage_state.State);
        }
    }
}