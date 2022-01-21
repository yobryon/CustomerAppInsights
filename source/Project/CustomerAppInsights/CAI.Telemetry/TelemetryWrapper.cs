using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;

namespace CAI.Telemetry
{
    public class TelemetryWrapper
    {
        ITelemetry Telemetry { get; set; }
        Action<ITelemetry> contextSendAction;
        Func<RequestTelemetry, IOperationHolder<RequestTelemetry>> contextStartAction;

        public TelemetryWrapper(ITelemetry telemetry, Action<ITelemetry> sendAction)
        {
            Telemetry = telemetry;
            contextSendAction = sendAction;
        }

        public TelemetryWrapper(ITelemetry telemetry, Func<RequestTelemetry, IOperationHolder<RequestTelemetry>> startOp)
        {
            Telemetry = telemetry;
            contextStartAction = startOp;
        }

        public TelemetryWrapper SetProperty(string key, string value)
        {
            Telemetry.Context.GlobalProperties[key] = value;
            return this;
        }

        public void Send() => contextSendAction(Telemetry);

        public IOperationHolder<RequestTelemetry> StartOperation(string operationName, string operationId = null)
        {
            RequestTelemetry requestTelemetry = (RequestTelemetry)Telemetry;
            requestTelemetry.Name = operationName;
            requestTelemetry.Id = operationId;
            return contextStartAction(requestTelemetry);
        }
    }
}
