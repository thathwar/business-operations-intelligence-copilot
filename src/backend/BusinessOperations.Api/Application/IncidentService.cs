using BusinessOperations.Api.Domain;

namespace BusinessOperations.Api.Application
{
    public class IncidentService
    {
        private readonly List<BusinessIncident> _incidents =
        [
            new BusinessIncident
        {
            Title = "VIP valet delivery SLA breach",
            Module = "OperationalSlaBreach",
            ScenarioType = "ValetDelay",
            Severity = "High",
            Status = "Detected",
            EstimatedBusinessImpact = 2500,
            ConfidenceScore = 86,
            Summary = "VIP customer vehicle retrieval exceeded SLA. Queue length is increasing."
        },
        new BusinessIncident
        {
            Title = "Customer service queue overload",
            Module = "OperationalSlaBreach",
            ScenarioType = "QueueOverload",
            Severity = "Medium",
            Status = "Investigating",
            EstimatedBusinessImpact = 1200,
            ConfidenceScore = 74,
            Summary = "Service queue wait time exceeded operational threshold during peak period."
        }
        ];

        private readonly List<AgentFinding> _findings = new();

        private readonly List<RecommendedAction> _actions = new();

        public IncidentService()
        {
            foreach (var incident in _incidents)
            {
                _findings.AddRange(new[]
                {
                    new AgentFinding
                    {
                        IncidentId = incident.Id,
                        AgentName = "RootCauseAgent",
                        FindingType = "RootCause",
                        Title = "Probable Root Cause",
                        Details = "Event traffic surge combined with reduced staffing caused queue growth and SLA breach risk.",
                        ConfidenceScore = 82
                    },
                    new AgentFinding
                    {
                        IncidentId = incident.Id,
                        AgentName = "BusinessImpactAgent",
                        FindingType = "BusinessImpact",
                        Title = "Business Impact",
                        Details = "VIP dissatisfaction risk is high. Potential compensation exposure and customer experience escalation.",
                        ConfidenceScore = 88
                    },
                    new AgentFinding
                    {
                        IncidentId = incident.Id,
                        AgentName = "RecommendationAgent",
                        FindingType = "Recommendation",
                        Title = "Recommended Actions",
                        Details = "Dispatch backup staff, open overflow queue lane, prioritize VIP retrieval, notify concierge operations team.",
                        ConfidenceScore = 85
                    }
                });

                _actions.AddRange(new[]
                {
                    new RecommendedAction
                    {
                        IncidentId = incident.Id,
                        ActionTitle = "Dispatch backup staff",
                        ActionDescription = "Assign two additional staff members to reduce queue pressure.",
                        Priority = "High",
                        RequiresApproval = false,
                        Status = "Pending"
                    },
                    new RecommendedAction
                    {
                        IncidentId = incident.Id,
                        ActionTitle = "Prioritize VIP retrieval",
                        ActionDescription = "Move VIP vehicle requests to priority queue.",
                        Priority = "High",
                        RequiresApproval = true,
                        Status = "Pending"
                    },
                    new RecommendedAction
                    {
                        IncidentId = incident.Id,
                        ActionTitle = "Notify operations manager",
                        ActionDescription = "Send incident summary to operations manager and concierge team.",
                        Priority = "Medium",
                        RequiresApproval = false,
                        Status = "Pending"
                    }
                });
            }


        }

        public IReadOnlyList<BusinessIncident> GetAll()
        {
            return _incidents;
        }

        public BusinessIncident? GetById(Guid id)
        {
            return _incidents.FirstOrDefault(x => x.Id == id);
        }

        public IReadOnlyList<AgentFinding> GetFindings(Guid incidentId)
        {
            return _findings.Where(x => x.IncidentId == incidentId).ToList();
        }

        public IReadOnlyList<RecommendedAction> GetRecommendedActions(Guid incidentId)
        {
            return _actions.Where(x => x.IncidentId == incidentId).ToList();
        }
    }
}
