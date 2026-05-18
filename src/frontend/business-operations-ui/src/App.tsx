import { useEffect, useState } from "react";

type Incident = {
  id: string;
  title: string;
  module: string;
  scenarioType: string;
  severity: string;
  status: string;
  estimatedBusinessImpact: number;
  confidenceScore: number;
  summary: string;
};

type AgentFinding = {
  id: string;
  incidentId: string;
  agentName: string;
  findingType: string;
  title: string;
  details: string;
  confidenceScore: number;
  createdAt: string;
};

type RecommendedAction = {
  id: string;
  incidentId: string;
  actionTitle: string;
  actionDescription: string;
  priority: string;
  requiresApproval: boolean;
  status: string;
};

export default function App() {
  const [incidents, setIncidents] = useState<Incident[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedIncidentId, setSelectedIncidentId] = useState<string | null>(null);
  const [findings, setFindings] = useState<AgentFinding[]>([]);
  const [actions, setActions] = useState<RecommendedAction[]>([]);

  useEffect(() => {
    fetch("https://localhost:7102/api/incidents")
      .then((res) => res.json())
      .then((data) => {
        console.log("Incidents loaded:", data);
        setIncidents(data);
      })
      .catch((error) => {
        console.error("Failed to load incidents", error);
      })
      .finally(() => setLoading(false));
  }, []);

  useEffect(() => {
    if (!selectedIncidentId) return;

    fetch(`https://localhost:7102/api/incidents/${selectedIncidentId}/findings`)
      .then((res) => res.json())
      .then((data) => setFindings(data))
      .catch((error) => console.error("Failed to load findings", error));
  }, [selectedIncidentId]);

  useEffect(() => {
    if (!selectedIncidentId) return;

    fetch(`https://localhost:7102/api/incidents/${selectedIncidentId}/actions`)
      .then((res) => res.json())
      .then((data) => setActions(data))
      .catch((error) => console.error("Failed to load actions", error));
  }, [selectedIncidentId]);

  const totalIncidents = incidents.length;
  const highSeverity = incidents.filter((x) => x.severity === "High").length;
  const totalImpact = incidents.reduce(
    (sum, x) => sum + x.estimatedBusinessImpact,
    0
  );
  const avgConfidence =
    incidents.length === 0
      ? 0
      : Math.round(
        incidents.reduce((sum, x) => sum + x.confidenceScore, 0) /
        incidents.length
      );

  const selectedIncident = incidents.find((x) => x.id === selectedIncidentId);
  const runAnalysis = async () => {
    if (!selectedIncidentId) return;

    const response = await fetch(
      `https://localhost:7102/api/incidents/${selectedIncidentId}/analyze`,
      {
        method: "POST",
      }
    );

    const data = await response.json();
    setFindings(data.filter(
  (value: AgentFinding, index: number, self: AgentFinding[]) =>
    index === self.findIndex(x => x.agentName === value.agentName)
));
  };
  return (
    <main className="min-h-screen bg-slate-100 p-8">
      <section className="mx-auto max-w-6xl">
        <h1 className="text-3xl font-bold text-slate-900">
          Business Operations Intelligence & Incident Copilot
        </h1>

        {loading && <p className="mt-8">Loading incidents...</p>}

        <div className="mt-8 grid grid-cols-1 gap-4 md:grid-cols-4">
          <div className="rounded-xl bg-white p-5 shadow-sm">
            <p className="text-sm text-slate-500">Active Incidents</p>
            <p className="mt-2 text-3xl font-bold">{totalIncidents}</p>
          </div>

          <div className="rounded-xl bg-white p-5 shadow-sm">
            <p className="text-sm text-slate-500">High Severity</p>
            <p className="mt-2 text-3xl font-bold">{highSeverity}</p>
          </div>

          <div className="rounded-xl bg-white p-5 shadow-sm">
            <p className="text-sm text-slate-500">Estimated Impact</p>
            <p className="mt-2 text-3xl font-bold">
              QAR {totalImpact.toLocaleString()}
            </p>
          </div>

          <div className="rounded-xl bg-white p-5 shadow-sm">
            <p className="text-sm text-slate-500">Avg Confidence</p>
            <p className="mt-2 text-3xl font-bold">{avgConfidence}%</p>
          </div>
        </div>

        <div className="mt-8 grid gap-4">
          {incidents.map((incident) => (
            <div
              key={incident.id}
              onClick={() => setSelectedIncidentId(incident.id)}
              className="cursor-pointer rounded-xl border bg-white p-5 shadow-sm hover:bg-slate-50"
            >
              <h2 className="text-xl font-semibold">{incident.title}</h2>
              <p>{incident.summary}</p>
              <p>Severity: {incident.severity}</p>
              <p>Status: {incident.status}</p>
            </div>
          ))}
          {selectedIncident && (
            <div className="mt-8 rounded-xl border bg-white p-6 shadow-sm">
              <h2 className="text-2xl font-bold text-slate-900">
                Incident Details
              </h2>

              <p className="mt-4 text-lg font-semibold">
                {selectedIncident.title}
              </p>

              <p className="mt-2 text-slate-600">
                {selectedIncident.summary}
              </p>

              <div className="mt-6 grid grid-cols-1 gap-4 text-sm">
                <p><b>Module:</b> {selectedIncident.module}</p>
                <p><b>Scenario:</b> {selectedIncident.scenarioType}</p>
                <p><b>Severity:</b> {selectedIncident.severity}</p>
                <p><b>Status:</b> {selectedIncident.status}</p>
                <p><b>Impact:</b> QAR {selectedIncident.estimatedBusinessImpact.toLocaleString()}</p>
                <p><b>Confidence:</b> {selectedIncident.confidenceScore}%</p>
             
               <div>
                    <button
                  onClick={runAnalysis}
                  className="rounded-lg bg-slate-900 px-4 py-2 text-sm font-semibold text-white hover:bg-slate-700"
                >
                  Run AI Analysis
                </button>
                </div>
                <div className="mt-8 rounded-lg bg-slate-50 p-5">
               
                  <h3 className="text-lg font-semibold text-slate-900">
                    AI Agent Findings
                  </h3>

                  <div className="mt-4 space-y-4 text-sm">
                    {findings.map((finding) => (
                      <div key={finding.id} className="rounded-lg bg-white p-4">
                        <p className="font-semibold text-slate-700">
                          {finding.title}
                        </p>

                        <p className="mt-1 text-slate-600">
                          {finding.details}
                        </p>

                        <p className="mt-2 text-xs text-slate-400">
                          {finding.agentName} · Confidence {finding.confidenceScore}%
                        </p>
                      </div>
                    ))}
                  </div>
                </div>
                <div className="mt-8 rounded-lg bg-slate-50 p-5">
                  <h3 className="text-lg font-semibold text-slate-900">
                    Recommended Actions
                  </h3>

                  <div className="mt-4 space-y-3 text-sm">
                    {actions.map((action) => (
                      <div key={action.id} className="rounded-lg bg-white p-4">
                        <div className="flex items-start justify-between">
                          <div>
                            <p className="font-semibold text-slate-700">
                              {action.actionTitle}
                            </p>

                            <p className="mt-1 text-slate-600">
                              {action.actionDescription}
                            </p>
                          </div>

                          <span className="rounded-full bg-slate-100 px-3 py-1 text-xs font-medium">
                            {action.priority}
                          </span>
                        </div>

                        <p className="mt-2 text-xs text-slate-400">
                          Status: {action.status} · Approval:{" "}
                          {action.requiresApproval ? "Required" : "Not required"}
                        </p>
                      </div>
                    ))}
                  </div>
                </div>
              </div>
            </div>
          )}
        </div>
      </section>
    </main>
  );
}