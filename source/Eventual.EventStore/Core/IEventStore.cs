using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eventual.EventStore.Core
{
    public interface IEventStore
    {
        Task CommitChangesAsync(EventStreamCommit eventStreamCommit);

        Task<IList<Revision>> GetEventStreamAsync(Guid aggregateId);

        Task<IList<Revision>> GetEventStreamFromAsync(Guid aggregateId, int initialRevision, int numberOfResults);

        Task<IList<Revision>> GetAllEventStreamsFromAsync(long initialCommit, int numberOfResults);
    }
}
