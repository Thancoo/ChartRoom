using System;

namespace ChatRoom.Entity.Operation
{
    public class OperationType:Base.EnitityBase
    {
        public int? Id { set;get;}
        public string Name { set;get;}
        public string Describe { set; get; }
    }
}
