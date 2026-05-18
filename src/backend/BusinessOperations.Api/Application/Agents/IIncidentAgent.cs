using BusinessOperations.Api.Domain;

namespace BusinessOperations.Api.Application.Agents
{
    public interface IIncidentAgent
    {
        string Name { get; }

        Task<AgentFinding> AnalyzeAsync(BusinessIncident incident);
    }

}
