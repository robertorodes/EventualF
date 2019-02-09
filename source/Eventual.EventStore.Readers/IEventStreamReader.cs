using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eventual.EventStore.Readers
{
    public interface IEventStreamReader
    {
        Task<EventStream> GetEventStreamFromAsync(string streamId, int initialRevisionId, int numberOfResults);

        Task<EventStream> GetAllEventStreamsFromAsync(long initialCommitId, int numberOfResults);
    }
}
