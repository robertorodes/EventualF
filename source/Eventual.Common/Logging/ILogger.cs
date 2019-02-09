using System;
using System.Collections.Generic;
using System.Text;

namespace Eventual.Common.Logging
{
    public interface ILogger
    {
        void LogInformation(string message);
        void LogInformation(string fmt, params object[] vars);
        void LogInformation(Exception exception, string fmt, params object[] vars);

        void LogWarning(string message);
        void LogWarning(string fmt, params object[] vars);
        void LogWarning(Exception exception, string fmt, params object[] vars);

        void LogError(string message);
        void LogError(string fmt, params object[] vars);
        void LogError(Exception exception, string fmt, params object[] vars);

        void TraceApiCall(string componentName, string method, TimeSpan timespan);
        void TraceApiCall(string componentName, string method, TimeSpan timespan, string properties);
        void TraceApiCall(string componentName, string method, TimeSpan timespan, string fmt, params object[] vars);
    }
}
