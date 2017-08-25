using System;
using ChatRoom.Common.Attribute;
using ChatRoom.Common.Enum;

namespace ChatRoom.Entity.Base
{
    public class EnitityBase
    {
        [OptionIgnore(RepositoryOption.Update)]
        public string CreatedBy { get; set; }
        [OptionIgnore(RepositoryOption.Update)]
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        [OptionIgnore(RepositoryOption.Insert | RepositoryOption.Update)]
        public bool? Available { get; set; }
        [OptionIgnore(RepositoryOption.Insert | RepositoryOption.Select | RepositoryOption.Delete | RepositoryOption.Update)]
        public int Rows { get; set; }
        [OptionIgnore(RepositoryOption.Insert | RepositoryOption.Select | RepositoryOption.Delete | RepositoryOption.Update)]
        public int Page { get; set; }
    }
}
