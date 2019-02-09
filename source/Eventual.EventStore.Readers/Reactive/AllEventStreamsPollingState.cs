using Eventual.EventStore.Core;
using System;
using System.Collections.Generic;

namespace Eventual.EventStore.Readers.Reactive
{
    class AllEventStreamsPollingState
    {
        public GlobalCheckpoint LastCheckpoint { get; set; }

        public IEnumerable<Revision> LastStreamBatch { get; set; }
    }
}
