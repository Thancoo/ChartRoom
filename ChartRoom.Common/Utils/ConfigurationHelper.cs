using System;
using System.Configuration;
using System.Text.RegularExpressions;
using Dapper;
using MySql.Data.MySqlClient;

namespace ChatRoom.Common.Utils
{
    public static class ConfigurationHelper
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public static int DefultGroupId => Convert.ToInt32(ConfigurationManager.AppSettings["DefultGroupId"] ?? "1");
        public static int VerifyExpiredDays => Convert.ToInt32(ConfigurationManager.AppSettings["VerifyExpired"] ?? "30");
        public static string DefaultHeadImageUrl => ConfigurationManager.AppSettings["Domain"] +
                                                    ConfigurationManager.AppSettings["DefaultHeardImageUrl"];
        public static string AuthTokenName => ConfigurationManager.AppSettings["AuthTokenName"];
        public static string VerifyTokenName => ConfigurationManager.AppSettings["VerifyTokenName"];
        public static double AuthTokenExpiredDays => Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpired"]??"30");
        public static string UserIdName =>ConfigurationManager.AppSettings["UserIdName"];
        public static string UserTypeVisitor => ConfigurationManager.AppSettings["UserType_Visitor"];
        public static string UserTypeGenericVip => ConfigurationManager.AppSettings["UserType_GenVip"];
        public static string UserTypeAdmin => ConfigurationManager.AppSettings["UserType_Admin"];

        public static int VerifyDbExpired => Convert.ToInt32(ConfigurationManager.AppSettings["VerifyDbExpired"] ?? "3");
        public static Regex EmjoyCodePat=>new Regex(ConfigurationManager.AppSettings["EmjoyCodePat"],RegexOptions.Compiled|RegexOptions.ECMAScript|RegexOptions.Multiline);
        public static Regex EmjoyElePat => new Regex(ConfigurationManager.AppSettings["EmjoyElePat"], RegexOptions.Compiled | RegexOptions.ECMAScript | RegexOptions.Multiline);
        public static Regex EmjoyCodePatRp => new Regex(ConfigurationManager.AppSettings["EmjoyCodePatRp"], RegexOptions.Compiled | RegexOptions.ECMAScript | RegexOptions.Multiline);
        public static Regex EmjoyElePatRp => new Regex(ConfigurationManager.AppSettings["EmjoyElePatRp"], RegexOptions.Compiled | RegexOptions.ECMAScript | RegexOptions.Multiline);
        public static Regex HtmlElePt => new Regex(ConfigurationManager.AppSettings["AllElePat"], RegexOptions.Compiled | RegexOptions.ECMAScript | RegexOptions.Multiline);
        public static string EncodeEmjoyTemplate=>ConfigurationManager.AppSettings["EncodeEmjoyTemplate"];
        public static string DecodeEmjoyTemplate=>ConfigurationManager.AppSettings["DecodeEmjoyTemplate"];
        public static int GenericGroupId=>Convert.ToInt32(ConfigurationManager.AppSettings["GenericGroupId"] ?? "2");

        public static MySqlConnection GetSqlConnection()
        {
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }
    }
}
