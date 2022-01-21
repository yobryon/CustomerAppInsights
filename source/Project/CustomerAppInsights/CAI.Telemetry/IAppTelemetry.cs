using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;

namespace CAI.Telemetry
{
    public interface IAppTelemetry
    {
        IAppTelemetry SetContext(string userId, string sessionId, bool inBatch);
        IAppTelemetry SetUser(string userId);
        IAppTelemetry SetSessionId(string sessionId);
        IAppTelemetry SetBatch(bool inBatch = true);
        IAppTelemetry SetProperty(string key, string value);

        // telemetry builders
        TelemetryWrapper TrackMetric(string _metricName, double _metricValue);
        TelemetryWrapper TrackPageView(string _pageName);
        TelemetryWrapper TrackException(string _ex, string _stack = null);
        TelemetryWrapper TrackException(Exception exception);
        TelemetryWrapper TrackException(ExceptionTelemetry telemetry);
        TelemetryWrapper TrackEvent(string _eventName);
        TelemetryWrapper TrackTrace(string message, SeverityLevel severityLevel);
        TelemetryWrapper TrackRequest();

        IOperationHolder<RequestTelemetry> TrackOperation(string operationName, string operationId = null);

        // to send built telemetry
        void SendTelemetry(ITelemetry telemetry);
    }
}
