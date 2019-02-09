using Eventual.EventStore.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventual.EventStore.Serialization
{
    public interface IEventsSerializer
    {
        byte[] Serialize(IEnumerable<Event> events);

        IEnumerable<Event> Deserialize(byte[] encodedEvents);

        string ContentType { get; }

        string ContentEncoding { get; }
    }
}
