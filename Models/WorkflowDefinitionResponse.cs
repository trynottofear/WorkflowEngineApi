namespace State_Machine_API.Models
{
    public class WorkflowDefinitionResponse
    {
        public Guid Id { get; set; }
        public List<WorkflowState> States { get; set; } = default!;
        public List<WorkflowAction> Actions { get; set; } = default!;
    }
}
