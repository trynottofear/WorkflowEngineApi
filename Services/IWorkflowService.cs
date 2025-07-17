using State_Machine_API.Models;

namespace State_Machine_API.Services
{
    // Services/IWorkflowService.cs
    public interface IWorkflowService
    {
        WorkflowDefinitionResponse CreateDefinition(CreateWorkflowDefinitionRequest req);
        WorkflowDefinitionResponse GetDefinition(Guid definitionId);
        IEnumerable<WorkflowDefinitionResponse> GetDefinitions();

        WorkflowInstance StartInstance(Guid definitionId);
        WorkflowInstance GetInstance(Guid instanceId);
        IEnumerable<WorkflowInstance> GetInstances();

        WorkflowInstance ExecuteAction(Guid instanceId, string actionId);
    }
}
