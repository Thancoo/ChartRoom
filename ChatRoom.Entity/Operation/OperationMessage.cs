namespace ChatRoom.Entity.Operation
{
    public class OperationMessage:Base.EnitityBase
    {
        public int? Id { get; set;}
        public int? OperationType { get; set;}
        public int? OperatorId { get; set;}
        public int? OperationTargetId { get; set;}
        public int? OperationState { get; set;}
        public string OperationAttach { get; set;}
    }
}
