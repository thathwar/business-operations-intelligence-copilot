namespace BusinessOperations.Api.Domain
{
    public class IncidentEvidence
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid IncidentId { get; set; }

        public string EvidenceType { get; set; } = string.Empty;
        // QueueMetric, Complaint, StaffAvailability, CustomerTier, SystemEvent

        public string SourceSystem { get; set; } = string.Empty;
        // ValetSystem, CRM, WhatsApp, POS, BuildingSystem

        public string Description { get; set; } = string.Empty;

        public decimal? NumericValue { get; set; }

        public string Unit { get; set; } = string.Empty;
        // minutes, customers, %, QAR

        public DateTime CapturedAt { get; set; } = DateTime.UtcNow;
    }
}
