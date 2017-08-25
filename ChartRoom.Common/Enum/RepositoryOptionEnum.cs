using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Common.Enum
{
    public enum RepositoryOption : uint
    {
        Insert = 0x1,
        Delete = 0x10,
        Select = 0x100,
        Update = 0x1000
    }
}
