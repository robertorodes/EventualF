using System;
using System.Runtime.Serialization;

namespace Eventual.EventStore.Readers
{
    public class EventStreamReaderException : Exception
    {
        public EventStreamReaderException() : base("A concurrency problem has occurred. Data has been changes since it was loaded from the source database.") { }

        public EventStreamReaderException(string message) : base(message) { }

        public EventStreamReaderException(string message, Exception innerException) : base(message, innerException) { }

        public EventStreamReaderException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}