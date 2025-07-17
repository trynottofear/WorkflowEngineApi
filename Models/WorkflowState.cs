namespace State_Machine_API.Models
{
    public class WorkflowState
    {
        public string Id { get; set; } = default!;
        public bool IsInitial { get; set; }
        public bool IsFinal { get; set; }
    }
}
