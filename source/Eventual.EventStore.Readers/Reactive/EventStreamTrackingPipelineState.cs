using Eventual.EventStore.Core;
using Eventual.EventStore.Readers.Tracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventual.EventStore.Readers.Reactive
{
    class EventStreamTrackingPipelineState
    {
        public EventStreamTracker Tracker { get; set; }

        public Revision Revision { get; set; }
    }
}
