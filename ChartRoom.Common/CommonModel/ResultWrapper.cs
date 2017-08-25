using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Common.CommonModel
{
    public class ResultWrapper
    {
        public ResultWrapper(){}
        /// <summary>
        /// 结果类型
        /// </summary>
        /// <param name="state">消息的状态</param>
        /// <param name="message">消息的内容</param>
        public ResultWrapper(bool state, string message)
        {
            this.State = state;
            this.Message = message;
        }
        /// <summary>
        /// 结果类型
        /// </summary>
        /// <param name="state">消息的状态</param>
        /// <param name="message">消息的内容</param>
        public ResultWrapper(bool state,int stateCode, string message)
        {
            this.State = state;
            this.StateCode = stateCode;
            this.Message = message;
        }
        public bool State { get; set; }
        public int StateCode { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
        public Exception Exception { get; set; }
    }
}
