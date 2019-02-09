using Eventual.EventStore.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventual.EventStore.Readers.Reactive
{
    class EventStreamPollingState
    {
        public EventStreamCheckpoint LastCheckpoint { get; set; }

        public IEnumerable<Revision> LastStreamBatch { get; set; }
    }
}
