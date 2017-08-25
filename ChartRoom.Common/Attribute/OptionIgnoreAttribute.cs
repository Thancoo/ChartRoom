using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatRoom.Common.Enum;

namespace ChatRoom.Common.Attribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class OptionIgnoreAttribute : System.Attribute
    {
        private RepositoryOption repositoryOption;
        public RepositoryOption ROption { get { return repositoryOption; } set { this.repositoryOption = value; } }
        public OptionIgnoreAttribute(RepositoryOption ro)
        {
            this.ROption = ro;
        }
    }
}
