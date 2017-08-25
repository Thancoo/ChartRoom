using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Common.Attribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PrimaryKeyAttribute : System.Attribute
    {
        private bool isPrimaryKey = true;
        public bool IsPrimaryKey
        {
            get { return this.isPrimaryKey; }
            set { this.isPrimaryKey = value; }
        }
        public PrimaryKeyAttribute(bool isprimarykey = true)
        {
            this.IsPrimaryKey = isprimarykey;
        }
    }
}
