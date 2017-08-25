using System.Linq;
using System.Web;

namespace ChatRoom.Common.Utils
{
    public static class MessageContentHelp
    {
        public static string EncodeEmjoy(this string message, string replaceModel)
        {
            if (string.IsNullOrEmpty(message))
                return message;
            if (!ConfigurationHelper.EmjoyElePat.IsMatch(message))
                return message;
            var spl=string.Join("",ConfigurationHelper.EmjoyElePatRp.Split(message).Select(pp=> ConfigurationHelper.EmjoyElePatRp.IsMatch(pp)? ConfigurationHelper.EmjoyElePat.Replace(pp, replaceModel):(pp.IsIlegalHtmlString()?HttpUtility.HtmlEncode(pp):pp)));
            return spl;
        }
        public static string DecodeEmjoy(this string message, string replaceModel)
        {
            if (string.IsNullOrEmpty(message))
                return message;
            if (!ConfigurationHelper.EmjoyCodePat.IsMatch(message))
                return message;
            var spl = string.Join("", ConfigurationHelper.EmjoyCodePatRp.Split(message).Select(pp => ConfigurationHelper.EmjoyCodePatRp.IsMatch(pp) ? ConfigurationHelper.EmjoyCodePat.Replace(pp, replaceModel) : (pp.IsIlegalHtmlString() ? HttpUtility.HtmlEncode(pp) : pp)));
            return spl;
        }

        private static bool IsIlegalHtmlString(this string conn)
        {
            return ConfigurationHelper.HtmlElePt.IsMatch(conn);
        }
    }
}
