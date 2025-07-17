using Microsoft.AspNetCore.Mvc;
using State_Machine_API.Models;
using State_Machine_API.Services;
using System;

namespace State_Machine_API.Controllers
{
    [ApiController]
    [Route("api/workflowDefinitions")]
    public class WorkflowDefinitionsController : ControllerBase
    {
        private readonly IWorkflowService _svc;
        public WorkflowDefinitionsController(IWorkflowService svc) { _svc = svc; }

        // Create a new workflow definition
        [HttpPost]
        public ActionResult<WorkflowDefinitionResponse> CreateDefinition(CreateWorkflowDefinitionRequest req)
        {
            try
            {
                var created = _svc.CreateDefinition(req);
                return CreatedAtAction(nameof(GetDefinition), new { definitionId = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Retrieve all definitions
        [HttpGet]
        public ActionResult<IEnumerable<WorkflowDefinitionResponse>> GetDefinitions()
        {
            var defs = _svc.GetDefinitions();
            return Ok(defs);
        }

        // Retrieve a specific definition by ID
        [HttpGet("{definitionId}")]
        public ActionResult<WorkflowDefinitionResponse> GetDefinition(Guid definitionId)
        {
            try
            {
                var def = _svc.GetDefinition(definitionId);
                return Ok(def);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
