namespace State_Machine_API.Models
{
    public class WorkflowAction
    {
        public string Id { get; set; } = default!;
        public bool Enabled { get; set; }
        public List<string> FromStates { get; set; } = new();
        public string ToState { get; set; } = default!;
    }
}
