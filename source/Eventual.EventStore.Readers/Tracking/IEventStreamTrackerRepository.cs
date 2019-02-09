using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eventual.EventStore.Readers.Tracking
{
    public interface IEventStreamTrackerRepository
    {
        Task<EventStreamTracker> GetEventStreamTracker(string trackerId);

        Task<EventStreamTracker> GetEventStreamTracker(string trackerId, bool refresh);

        Task CreateEventStreamTracker(EventStreamTracker tracker);

        Task UpdateEventStreamTracker(EventStreamTracker tracker);

        Task CommitChanges();
    }
}
