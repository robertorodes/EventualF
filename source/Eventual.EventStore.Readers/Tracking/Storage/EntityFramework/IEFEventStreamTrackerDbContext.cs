using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventual.EventStore.Readers.Tracking.Storage.EntityFramework
{
    public interface IEFEventStreamTrackerDbContext
    {
        DbSet<EventStreamTracker> EventStreamTrackers { get; set; }
    }
}
