namespace State_Machine_API.Models
{
    public class CreateWorkflowDefinitionRequest
    {
        public List<WorkflowState> States { get; set; } = new();
        public List<WorkflowAction> Actions { get; set; } = new();
    }
}
