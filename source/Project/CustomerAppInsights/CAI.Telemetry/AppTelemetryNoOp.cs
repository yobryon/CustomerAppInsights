using System;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace CAI.Telemetry
{
    public class AppTelemetryNoOp : IAppTelemetry
    {
        public AppTelemetryNoOp()
        {

        }
        public IAppTelemetry SetBatch(bool inBatch = true)
        {
            return this;
        }

        public IAppTelemetry SetContext(string userId, string sessionId, bool inBatch)
        {
            return this;
        }

        public IAppTelemetry SetSessionId(string sessionId)
        {
            return this;
        }

        public IAppTelemetry SetUser(string userId)
        {
            return this;
        }

        private TelemetryWrapper Wrap(ITelemetry telemetry) => new TelemetryWrapper(telemetry, t => SendTelemetry(t));
        public TelemetryWrapper TrackMetric(string _metricName, double _metricValue) => Wrap(new MetricTelemetry(_metricName, _metricValue));
        public TelemetryWrapper TrackPageView(string _pageName) => Wrap(new PageViewTelemetry(_pageName));
        public TelemetryWrapper TrackException(string _ex, string _stack = null) => TrackException(new AxException(_ex, _stack));
        public TelemetryWrapper TrackException(Exception exception) => TrackException(new ExceptionTelemetry(exception));
        public TelemetryWrapper TrackException(ExceptionTelemetry telemetry) => Wrap(telemetry);
        public TelemetryWrapper TrackEvent(string _eventName) => Wrap(new EventTelemetry(_eventName));
        public TelemetryWrapper TrackTrace(string message, SeverityLevel severityLevel) => Wrap(new TraceTelemetry(message, severityLevel));
        public TelemetryWrapper TrackRequest() => new TelemetryWrapper(new RequestTelemetry(), _ => new NoOpOperationHolder());

        public IOperationHolder<RequestTelemetry> TrackOperation(string operationName, string operationId = null) => new NoOpOperationHolder();

        public IAppTelemetry SetProperty(string key, string value) => this;

        public void SendTelemetry(ITelemetry telemetry) { }

    }

    public class NoOpOperationHolder : IOperationHolder<RequestTelemetry>, IDisposable
    {
        public NoOpOperationHolder()
        {

        }
        public RequestTelemetry Telemetry { get; } = new RequestTelemetry();

        public void Dispose()
        {
            
        }
    }
}
