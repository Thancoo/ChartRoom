using System;
using System.Text.RegularExpressions;
using ChatRoom.Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;

namespace ChatRoom.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var st= @"sdfjaoefaos我就是想试一试表情<img src=""../../Images/emjoy/19.gif"">被格式化";
            var reg= new Regex(@"<img src=""[a-zA-Z0-9/\.]+Images/emjoy/([0-9]+)\.gif"">",RegexOptions.ECMAScript | RegexOptions.Multiline);
            var oo= reg.Split(st);
            //Assert.IsTrue(reg.IsMatch(st));
            var res = reg.Replace(st, "[emj_$1]");
        }
        [TestMethod]
        public void TestMethod2()
        {
            var st= @"sdfjaoefaos我就是想试一试表情<img src=""../../Images/emjoy/19.gif"">被格式化";
            var res = st.EncodeEmjoy(ConfigurationHelper.EncodeEmjoyTemplate);
        }
        [TestMethod]
        public void TestMethod3()
        {
            var st = @"sdfjaoefaos我就是想试一试表情[emj_19]被格式化";
            var reg = new Regex(@"<img src=""[a-zA-Z0-9/\.]+Images/emjoy/([0-9]+)\.gif"">", RegexOptions.ECMAScript | RegexOptions.Multiline);
            Assert.IsTrue(reg.IsMatch(st));
            var str=reg.Split(st);
            var res = reg.Replace(st, "[emj_$1]");
        }
        [TestMethod]
        public void TestMethod4()
        {
            var st = @"sdfjaoefaos我就是想试一试表情[emj_19]被格式化";
            var res = st.DecodeEmjoy(ConfigurationHelper.DecodeEmjoyTemplate);
        }
    }
}
