using BusinessOperations.Api.Application.AI;
using BusinessOperations.Api.Domain;
using System.Text.Json;

namespace BusinessOperations.Api.Application.Agents
{
    public class BusinessImpactAgent : IIncidentAgent
    {
        private readonly OpenAiService _openAiService;

        public BusinessImpactAgent(OpenAiService openAiService)
        {
            _openAiService = openAiService;
        }

        public string Name => "BusinessImpactAgent";

        public async Task<AgentFinding> AnalyzeAsync(BusinessIncident incident)
        {
            var prompt = $$"""
                            You are a business impact analysis agent.

                            Analyze the customer and operational business impact.

                            Incident:
                            Title: {incident.Title}
                            Module: {incident.Module}
                            Scenario Type: {incident.ScenarioType}
                            Severity: {incident.Severity}
                            Status: {incident.Status}
                            Estimated Business Impact: {incident.EstimatedBusinessImpact}
                            Summary: {incident.Summary}

                            Return ONLY valid JSON.
                            Format:
                            {
    
                                  "details": "2-3 sentence business impact analysis",
                              "confidenceScore": 0
                            }

                            Confidence score must be between 0 and 100.
                            Focus on customer impact, SLA breach, VIP escalation, revenue exposure, reputation risk.
                            """;

            var result = await _openAiService.AnalyzeIncidentAsync(prompt);
            var json = JsonDocument.Parse(result);

            return new AgentFinding
            {
                IncidentId = incident.Id,
                AgentName = Name,
                FindingType = "BusinessImpact",
                Title = "Business Impact",
                Details = json.RootElement.GetProperty("details").GetString() ?? "",
                ConfidenceScore = json.RootElement.GetProperty("confidenceScore").GetInt32()
            };
        }
    }
}
