using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Eventual.Common.Logging.DiagnosticsTraceSource
{
    public class DiagnosticsTraceSourceLogger : ILogger
    {
        #region Private properties

        private TraceSource TraceSource { get; set; }

        #endregion

        #region Constructors

        public DiagnosticsTraceSourceLogger(string traceSourceName)
        {
            this.TraceSource = new TraceSource(traceSourceName);
        }

        #endregion

        #region Information logging - Trace information within the application

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void LogInformation(string message)
        {
            message = string.Format("{0} : {1}", this.GetCallerName(2), message);
            this.TraceSource.TraceInformation(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void LogInformation(string format, params object[] vars)
        {
            string finalFormat = string.Format("{0} : {1}", this.GetCallerName(2), format);
            this.TraceSource.TraceInformation(finalFormat, vars);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void LogInformation(Exception exception, string format, params object[] vars)
        {
            string finalFormat = string.Format("{0} : {1}", this.GetCallerName(2), format);
            var msg = String.Format(finalFormat, vars);
            this.TraceSource.TraceInformation(msg + " : Exception Details={0}", ExceptionUtils.FormatException(exception, includeContext: true));
        }

        #endregion

        #region Warning logging - Trace warnings within the application 

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void LogWarning(string message)
        {
            message = string.Format("{0} : {1}", this.GetCallerName(2), message);
            this.TraceSource.TraceEvent(TraceEventType.Warning, 0, message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void LogWarning(string format, params object[] vars)
        {
            string finalFormat = string.Format("{0} : {1}", this.GetCallerName(2), format);
            this.TraceSource.TraceEvent(TraceEventType.Warning, 0, finalFormat, vars);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void LogWarning(Exception exception, string format, params object[] vars)
        {
            string finalFormat = string.Format("{0} : {1}", this.GetCallerName(2), format);
            var msg = String.Format(finalFormat, vars);
            this.TraceSource.TraceEvent(TraceEventType.Warning, 0, msg + " : Exception Details={0}", ExceptionUtils.FormatException(exception, includeContext: true));
        }

        #endregion

        #region Error logging - Trace fatal errors within the application

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void LogError(string message)
        {
            message = string.Format("{0} : {1}", this.GetCallerName(2), message);
            this.TraceSource.TraceEvent(TraceEventType.Error, 0, message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void LogError(string format, params object[] vars)
        {
            string finalFormat = string.Format("{0} : {1}", this.GetCallerName(2), format);
            this.TraceSource.TraceEvent(TraceEventType.Error, 0, finalFormat, vars);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void LogError(Exception exception, string format, params object[] vars)
        {
            string finalFormat = string.Format("{0} : {1}", this.GetCallerName(2), format);
            var msg = String.Format(finalFormat, vars);
            this.TraceSource.TraceEvent(TraceEventType.Error, 0, msg + " : Exception Details={0}", ExceptionUtils.FormatException(exception, includeContext: true));
        }

        #endregion

        #region API calls tracing - Trace inter-service calls (including latency)

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TraceApiCall(string componentName, string method, TimeSpan timespan)
        {
            //TraceApiCall(componentName, method, timespan, "");

            string message = String.Concat("component:", componentName, ";method:", method, ";timespan:", timespan.ToString());
            message = string.Format("{0} : {1}", this.GetCallerName(2), message);
            this.TraceSource.TraceInformation(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TraceApiCall(string componentName, string method, TimeSpan timespan, string fmt, params object[] properties)
        {
            //TraceApiCall(componentName, method, timespan, string.Format(fmt, properties));

            string propertiesString = string.Format(fmt, properties);
            string message = String.Concat("component:", componentName, ";method:", method, ";timespan:", timespan.ToString(), ";properties:", propertiesString);
            message = string.Format("{0} : {1}", this.GetCallerName(2), message);
            this.TraceSource.TraceInformation(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TraceApiCall(string componentName, string method, TimeSpan timespan, string properties)
        {
            string message = String.Concat("component:", componentName, ";method:", method, ";timespan:", timespan.ToString(), ";properties:", properties);
            message = string.Format("{0} : {1}", this.GetCallerName(2), message);
            this.TraceSource.TraceInformation(message);
        }

        #endregion

        #region Private helper methods

        [MethodImpl(MethodImplOptions.NoInlining)]
        private string GetCallerName(int framesToSkip)
        {
            StackFrame stackFrame = new StackFrame(framesToSkip, true);
            var method = stackFrame.GetMethod();
            string type = method.DeclaringType.ToString();
            string methodName = method.Name;

            string callerName = string.Format("{0}.{1}", type, methodName);

            return callerName;
        }

        #endregion
    }
}
