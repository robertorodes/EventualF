using System;
using System.Runtime.Serialization;

namespace Eventual.EventStore.Readers.Tracking
{
    public class EventStreamTrackedReaderDbException : EventStreamReaderException
    {
        public EventStreamTrackedReaderDbException() : base("A database problem has occurred.") { }

        public EventStreamTrackedReaderDbException(string message) : base(message) { }

        public EventStreamTrackedReaderDbException(string message, Exception innerException) : base(message, innerException) { }

        public EventStreamTrackedReaderDbException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}