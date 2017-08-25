namespace ChatRoom.Model.Operation
{
    public class OperationMessage:Base.ModelBase
    {
        public int? Id { get; set;}
        public int? OperationType { get; set;}
        public int? OperatorId { get; set;}
        public int? OperationTargetId { get; set;}
        public int? OperationState { get; set;}
        public string OperationAttach { get; set;}
    }
}
