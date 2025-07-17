namespace State_Machine_API.Models
{
    public class WorkflowInstance
    {
        public Guid Id { get; set; } 
        public Guid DefinitionId { get; set; }    
        public string CurrentStateId { get; set; } = default!;
        public List<InstanceHistoryEntry> History { get; set; } = new();
    }
    public record InstanceHistoryEntry
    {
        public string ActionId { get; set; } = default!;
        public DateTime Timestamp { get; set; }
    }
}
