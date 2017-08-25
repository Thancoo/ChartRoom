using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ChatRoom.Common.CommonModel;
using ChatRoom.Common.RequestModel;
using ChatRoom.Controllers.Base;
using ChatRoom.Interface.IBuiness.Message;

namespace ChatRoom.Controllers
{
    public class MessageController : ApiControllerBase
    {
        private readonly IMessageBuiness _messageBll;

        public MessageController(IMessageBuiness messageBll)
        {
            this._messageBll = messageBll;
        }
        public List<TransMessageModel> GetMessages()
        {
            //要不要提供这个接口？？暂时不提供
            return _messageBll.GetMessageDetails(new TransMessageModel()).ToList();
        }
        [HttpPost]
        public ResultWrapper GetGroupChatContext()
        {
            var res= this._messageBll.GetGroupChatContext(UserAuthContxt.User.Id);
            if(res!=null)
                return new ResultWrapper(true,"获取group聊天上下文成功！") {Data = res};
            return new ResultWrapper(false,"获取group聊天上下文失败！");
        }
    }
}
