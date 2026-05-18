using BusinessOperations.Api.Application;
using BusinessOperations.Api.Application.Agents;
using Microsoft.AspNetCore.Mvc;

namespace BusinessOperations.Api.Controllers
{
    [ApiController]
    [Route("api/incidents")]
    public class IncidentsController : ControllerBase
    {
        private readonly IncidentService _incidentService;

        private readonly IEnumerable<IIncidentAgent> _agents;
        public IncidentsController(IncidentService incidentService, IEnumerable<IIncidentAgent> agents)
        {
            _incidentService = incidentService;
            _agents = agents;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var incidents = _incidentService.GetAll();
            return Ok(incidents);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            var incident = _incidentService.GetById(id);

            if (incident is null)
                return NotFound();

            return Ok(incident);
        }

        [HttpGet("{id:guid}/findings")]
        public IActionResult GetFindings(Guid id)
        {
            var incident = _incidentService.GetById(id);

            if (incident is null)
                return NotFound();

            var findings = _incidentService.GetFindings(id);

            return Ok(findings);
        }

        [HttpGet("{id:guid}/actions")]
        public IActionResult GetActions(Guid id)
        {
            var incident = _incidentService.GetById(id);

            if (incident is null)
                return NotFound();

            var actions = _incidentService.GetRecommendedActions(id);

            return Ok(actions);
        }

        [HttpPost("{id:guid}/analyze")]
        public async Task<IActionResult> Analyze(Guid id)
        {
            var incident = _incidentService.GetById(id);

            if (incident is null)
                return NotFound();

            var findings = new List<object>();

            foreach (var agent in _agents)
            {
                var finding = await agent.AnalyzeAsync(incident);
                findings.Add(finding);
            }

            return Ok(findings);
        }
    }
}
