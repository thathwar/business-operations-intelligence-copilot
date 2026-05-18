namespace BusinessOperations.Api.Domain
{
    public class RecommendedAction
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid IncidentId { get; set; }

        public string ActionTitle { get; set; } = string.Empty;

        public string ActionDescription { get; set; } = string.Empty;

        public string Priority { get; set; } = "Medium";
        // Low, Medium, High

        public bool RequiresApproval { get; set; }

        public string Status { get; set; } = "Pending";
        // Pending, Approved, Rejected, Completed
    }
}
