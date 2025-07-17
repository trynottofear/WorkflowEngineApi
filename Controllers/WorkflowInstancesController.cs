using Microsoft.AspNetCore.Mvc;
using State_Machine_API.Models;
using State_Machine_API.Services;

namespace State_Machine_API.Controllers
{
    [ApiController]
    [Route("api/workflowInstances")]
    public class WorkflowInstancesController : ControllerBase
    {
        private readonly IWorkflowService _svc;
        public WorkflowInstancesController(IWorkflowService svc) { _svc = svc; }

        // Start a new instance for a given definition
        [HttpPost]
        public ActionResult<WorkflowInstance> StartInstance([FromQuery] Guid definitionId)
        {
            try
            {
                var inst = _svc.StartInstance(definitionId);
                return CreatedAtAction(nameof(GetInstance), new { instanceId = inst.Id }, inst);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // List all instances
        [HttpGet]
        public ActionResult<IEnumerable<WorkflowInstance>> GetInstances()
        {
            var list = _svc.GetInstances();
            return Ok(list);
        }

        // Get current state/history of one instance
        [HttpGet("{instanceId}")]
        public ActionResult<WorkflowInstance> GetInstance(Guid instanceId)
        {
            try
            {
                var inst = _svc.GetInstance(instanceId);
                return Ok(inst);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // Execute an action on an instance (via POST to avoid breaking REST style) 
        // For REST, we use a sub-resource "actions" on the instance URI (no verb in path):contentReference[oaicite:7]{index=7}.
        [HttpPost("{instanceId}/actions")]
        public ActionResult<WorkflowInstance> ExecuteAction(Guid instanceId, [FromBody] string actionId)
        {
            try
            {
                var updated = _svc.ExecuteAction(instanceId, actionId);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
