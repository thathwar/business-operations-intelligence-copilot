namespace BusinessOperations.Api.Domain
{
  
    public class BusinessIncident
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = string.Empty;

        public string Module { get; set; } = string.Empty;
        // Example: OperationalSlaBreach

        public string ScenarioType { get; set; } = string.Empty;
        // Example: ValetDelay, QueueOverload, ServiceDelay

        public string Severity { get; set; } = "Medium";
        // Low, Medium, High, Critical

        public string Status { get; set; } = "Detected";
        // Detected, Investigating, WaitingForAction, Resolved, Closed

        public decimal EstimatedBusinessImpact { get; set; }

        public int ConfidenceScore { get; set; }

        public DateTime DetectedAt { get; set; } = DateTime.UtcNow;

        public string Summary { get; set; } = string.Empty;
    }
}
