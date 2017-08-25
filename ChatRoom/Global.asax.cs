using System;
using System.Web.Http;
using ChatRoom.Common;
using ChatRoom.Common.Utils;

namespace ChatRoom
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            MessageListener.Instance().Start();
        }
        protected void Application_End()
        {
            MessageListener.Instance().Stop();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 2880;
            LogHelper.WriteLog(this.GetType(), $"开始会话-->UserId:{(Context.Request.Cookies[ConfigurationHelper.UserIdName]?.Value??"匿名")}");
        }
        protected void Session_End(object sender, EventArgs e)
        {
            LogHelper.WriteLog(this.GetType(), $"结束会话-->UserId:{(Context.Request.Cookies[ConfigurationHelper.UserIdName]?.Value ?? "匿名")}");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex != null && !string.IsNullOrEmpty(ex.Message))
            {
                try
                {
                    string err = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "\t错误源:" + ex.Source + "堆栈信息:" +
                                 ex.StackTrace + "异常信息:" + ex.InnerException + "信息：" + ex.Message;
                    LogHelper.WriteLog(this.GetType(), err);
                }
                catch (Exception)
                {
                }
            }
            Server.ClearError();
        }
    }
}
