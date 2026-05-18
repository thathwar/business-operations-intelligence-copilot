namespace BusinessOperations.Api.Domain
{
    public class AgentFinding
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid IncidentId { get; set; }

        public string AgentName { get; set; } = string.Empty;
        // RootCauseAgent, BusinessImpactAgent, RecommendationAgent

        public string FindingType { get; set; } = string.Empty;
        // RootCause, BusinessImpact, Recommendation

        public string Title { get; set; } = string.Empty;

        public string Details { get; set; } = string.Empty;

        public int ConfidenceScore { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
