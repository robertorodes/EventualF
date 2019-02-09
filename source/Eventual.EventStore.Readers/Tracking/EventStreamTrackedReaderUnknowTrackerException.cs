using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Eventual.EventStore.Readers.Tracking
{
    public class EventStoreClientUnknownTrackerException : EventStreamReaderException
    {
        public EventStoreClientUnknownTrackerException() : base("The specified tracker does not exist.") { }

        public EventStoreClientUnknownTrackerException(string message) : base(message) { }

        public EventStoreClientUnknownTrackerException(string message, Exception innerException) : base(message, innerException) { }

        public EventStoreClientUnknownTrackerException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
