namespace State_Machine_API.Models
{
    public class WorkflowDefinition
    {
        public Guid Id { get; set; }
        public List<WorkflowState> States { get; set; } = new();
        public List<WorkflowAction> Actions { get; set; } = new();
    }
}
