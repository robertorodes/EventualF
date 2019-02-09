using System;
using System.Collections.Generic;
using System.Text;

namespace Eventual.EventStore.Readers
{
    public enum CheckpointType
    {
        GlobalCheckpoint,
        StreamCheckpoint
    }
}
