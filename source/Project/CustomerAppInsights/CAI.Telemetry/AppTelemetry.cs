using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Channel;

namespace CAI.Telemetry
{
    public class AppTelemetry : IAppTelemetry
    {
        TelemetryClient cli;

        public AppTelemetry(string instrumentationKey)
        {
            cli = new TelemetryClient();
            cli.InstrumentationKey = instrumentationKey;
            SetExecutionMode("NotSpecified");
        }

        ~AppTelemetry()
        {
            cli.Flush();
        }

        public IAppTelemetry SetContext(string userId, string sessionId, bool inBatch)
        {
            return this.SetUser(userId).SetSessionId(sessionId).SetBatch(inBatch);
        }

        public IAppTelemetry SetUser(string userId)
        {
            cli.Context.User.Id = userId;
            return this;
        }

        public IAppTelemetry SetSessionId(string sessionId)
        {
            cli.Context.Session.Id = sessionId;
            return this;
        }

        public IAppTelemetry SetBatch(bool inBatch = true)
        {
            SetExecutionMode(inBatch ? "Batch" : "Interactive");
            return this;
        }

        private void SetExecutionMode(string mode)
        {
            SetProperty("ExecutionMode", mode);
        }
        public IAppTelemetry SetProperty(string key, string value)
        {
            cli.Context.GlobalProperties[key] = value;
            return this;
        }


        public TelemetryWrapper TrackMetric(string _metricName, double _metricValue) => Wrap(new MetricTelemetry(_metricName, _metricValue));
        public TelemetryWrapper TrackPageView(string _pageName) => Wrap(new PageViewTelemetry(_pageName));
        public TelemetryWrapper TrackException(string _ex, string _stack = null) => TrackException(new AxException(_ex, _stack));
        public TelemetryWrapper TrackException(Exception exception) => TrackException(new ExceptionTelemetry(exception));
        public TelemetryWrapper TrackException(ExceptionTelemetry telemetry) => Wrap(telemetry);
        public TelemetryWrapper TrackEvent(string _eventName) => Wrap(new EventTelemetry(_eventName));
        public TelemetryWrapper TrackTrace(string message, SeverityLevel severityLevel) => Wrap(new TraceTelemetry(message, severityLevel));

        public TelemetryWrapper TrackRequest() => new TelemetryWrapper(new RequestTelemetry(), t => cli.StartOperation(t));
        public IOperationHolder<RequestTelemetry> TrackOperation(string operationName, string operationId = null) => cli.StartOperation<RequestTelemetry>(operationName, operationId);
        
        private TelemetryWrapper Wrap(ITelemetry telemetry) => new TelemetryWrapper(telemetry, t => SendTelemetry(t));

        public void SendTelemetry(ITelemetry telemetry)
        {
            switch (telemetry)
            {
                case AvailabilityTelemetry availabilityTelemetry:
                    cli.TrackAvailability(availabilityTelemetry);
                    break;
                case DependencyTelemetry dependencyTelemetry:
                    cli.TrackDependency(dependencyTelemetry);
                    break;
                case EventTelemetry eventTelemetry:
                    cli.TrackEvent(eventTelemetry);
                    break;
                case ExceptionTelemetry exceptionTelemetry:
                    cli.TrackException(exceptionTelemetry);
                    break;
                case MetricTelemetry metricTelemetry:
                    cli.TrackMetric(metricTelemetry);
                    break;
                case PageViewTelemetry pageViewTelemetry:
                    cli.TrackPageView(pageViewTelemetry);
                    break;
                case RequestTelemetry requestTelemetry:
                    cli.TrackRequest(requestTelemetry);
                    break;
                case TraceTelemetry traceTelemetry:
                    cli.TrackTrace(traceTelemetry);
                    break;
                default:
                    //noop
                    break;
            }
        }

    }

}
