using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatRoom.Filter
{
    public class CustomerAllowAnonymousAttribute:Attribute
    {
        /// <summary>
        /// 多个角色之间用“,”分隔
        /// </summary>
        private string OpenFor { get; set; }

        public CustomerAllowAnonymousAttribute()
        {
        }
    }
}