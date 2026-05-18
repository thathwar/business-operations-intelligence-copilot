using BusinessOperations.Api.Application.AI;
using BusinessOperations.Api.Domain;
using System.Text.Json;

namespace BusinessOperations.Api.Application.Agents
{
    public class RootCauseAgent : IIncidentAgent
    {
        private readonly OpenAiService _openAiService;

        public RootCauseAgent(OpenAiService openAiService)
        {
            _openAiService = openAiService;
        }

        public string Name => "RootCauseAgent";

        public async Task<AgentFinding> AnalyzeAsync(BusinessIncident incident)
        {
            var prompt = $$"""
                        You are a business operations root cause analysis agent.

                        Analyze this operational incident.

                        Incident:
                        Title: {{incident.Title}}
                        Module: {{incident.Module}}
                        Scenario Type: {{incident.ScenarioType}}
                        Severity: {{incident.Severity}}
                        Status: {{incident.Status}}
                        Estimated Business Impact: {{incident.EstimatedBusinessImpact}}
                        Summary: {{incident.Summary}}

                        Return ONLY valid JSON.
                        Format:
                        {"details": "2-3 sentence root cause analysis",
                          "confidenceScore": 0
                        }

                        Confidence score must be between 0 and 100.

                        Focus on:
                        - staffing
                        - queues
                        - customer operations
                        - SLA
                        - business impact
                        - escalation risk

                        Do not discuss infrastructure or servers.
                        """;

            var result = await _openAiService.AnalyzeIncidentAsync(prompt);

            var json = JsonDocument.Parse(result);

            var details = json.RootElement
                .GetProperty("details")
                .GetString() ?? "";

            var confidence = json.RootElement
                .GetProperty("confidenceScore")
                .GetInt32();

            return new AgentFinding
            {
                IncidentId = incident.Id,
                AgentName = Name,
                FindingType = "RootCause",
                Title = "Probable Root Cause",
                Details = details,
                ConfidenceScore = confidence
            };
        }
    }
}
