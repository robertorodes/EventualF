using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eventual.EventStore.Services
{
    public interface ITypedEventStore
    {
        Task CommitChangesAsync(TypedEventStreamCommit eventStreamForCommit);

        Task<IEnumerable<EsRevision>> GetEventStreamAsync(Guid aggregateId);

        Task<IEnumerable<EsRevision>> GetEventStreamFromAsync(Guid aggregateId, int initialRevision, int numberOfResults);

        Task<IEnumerable<EsRevision>> GetAllEventStreamsFromAsync(int initialCommit, int numberOfResults);
    }
}
