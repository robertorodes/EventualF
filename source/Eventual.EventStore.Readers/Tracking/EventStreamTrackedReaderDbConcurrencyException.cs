using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Eventual.EventStore.Readers.Tracking
{
    public class EventStreamTrackedReaderDbConcurrencyException : EventStreamTrackedReaderDbException
    {
        public const string DefaultMessage = "A concurrency problem has occurred. Data has been changed since it was loaded from the origin database.";

        public EventStreamTrackedReaderDbConcurrencyException() : base() { }

        public EventStreamTrackedReaderDbConcurrencyException(string message) : base(message) { }

        public EventStreamTrackedReaderDbConcurrencyException(string message, Exception innerException) : base(message, innerException) { }

        public EventStreamTrackedReaderDbConcurrencyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
