using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Eventual.EventStore.Readers.Tracking
{
    public class EventStoreClientInvalidTrackerCheckpointException : EventStreamReaderException
    {
        public EventStoreClientInvalidTrackerCheckpointException() : base("The specified tracker checkpoint does not exist.") { }

        public EventStoreClientInvalidTrackerCheckpointException(string message) : base(message) { }

        public EventStoreClientInvalidTrackerCheckpointException(string message, Exception innerException) : base(message, innerException) { }

        public EventStoreClientInvalidTrackerCheckpointException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
