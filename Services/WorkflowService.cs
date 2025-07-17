using State_Machine_API.Models;
using State_Machine_API.Services;

namespace State_Machine_API.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly Dictionary<Guid, WorkflowDefinition> _definitions = new();
        private readonly Dictionary<Guid, WorkflowInstance> _instances = new();

        public WorkflowDefinitionResponse CreateDefinition(CreateWorkflowDefinitionRequest req)
        {
            // Validate: unique state IDs, one initial state, etc.
            if (req.States == null || req.States.Count == 0)
                throw new ApplicationException("At least one state is required.");
            if (req.States.Count(s => s.IsInitial) != 1)
                throw new ApplicationException("Exactly one initial state required.");
            // Additional validations: duplicate IDs, action validity, etc.

            var def = new WorkflowDefinition
            {
                Id = Guid.NewGuid(),
                States = req.States,
                Actions = req.Actions
            };
            _definitions[def.Id] = def;
            return new WorkflowDefinitionResponse
            {
                Id = def.Id,
                States = def.States,
                Actions = def.Actions
            };
        }

        public WorkflowDefinitionResponse GetDefinition(Guid definitionId)
        {
            if (!_definitions.TryGetValue(definitionId, out var def))
                throw new KeyNotFoundException("Definition not found.");
            return new WorkflowDefinitionResponse
            {
                Id = def.Id,
                States = def.States,
                Actions = def.Actions
            };
        }

        public IEnumerable<WorkflowDefinitionResponse> GetDefinitions() =>
            _definitions.Values.Select(def => new WorkflowDefinitionResponse
            {
                Id = def.Id,
                States = def.States,
                Actions = def.Actions
            });

        public WorkflowInstance StartInstance(Guid definitionId)
        {
            if (!_definitions.TryGetValue(definitionId, out var def))
                throw new KeyNotFoundException("Definition not found.");
            var initial = def.States.First(s => s.IsInitial);
            var inst = new WorkflowInstance
            {
                Id = Guid.NewGuid(),
                DefinitionId = def.Id,
                CurrentStateId = initial.Id
            };
            _instances[inst.Id] = inst;
            return inst;
        }

        public IEnumerable<WorkflowInstance> GetInstances() =>
            _instances.Values;

        public WorkflowInstance GetInstance(Guid instanceId)
        {
            if (!_instances.TryGetValue(instanceId, out var inst))
                throw new KeyNotFoundException("Instance not found.");
            return inst;
        }

        public WorkflowInstance ExecuteAction(Guid instanceId, string actionId)
        {
            if (!_instances.TryGetValue(instanceId, out var inst))
                throw new KeyNotFoundException("Instance not found.");
            if (!_definitions.TryGetValue(inst.DefinitionId, out var def))
                throw new KeyNotFoundException("Definition not found.");

            if (def.States.First(s => s.Id == inst.CurrentStateId).IsFinal)
                throw new ApplicationException("Cannot execute action: instance is in final state.");
            var action = def.Actions.FirstOrDefault(a => a.Id == actionId);
            if (action == null || !action.Enabled)
                throw new ApplicationException("Invalid or disabled action.");
            if (!action.FromStates.Contains(inst.CurrentStateId))
                throw new ApplicationException("Action cannot be executed from the current state.");
            // Perform transition
            var fromState = inst.CurrentStateId;
            var toState = action.ToState;
            inst.CurrentStateId = action.ToState;
            inst.History.Add(new InstanceHistoryEntry
            {
                ActionId = actionId,
                FromState = fromState,
                ToState = toState,
                Timestamp = DateTime.UtcNow
            });
            return inst;
        }
    }
}
