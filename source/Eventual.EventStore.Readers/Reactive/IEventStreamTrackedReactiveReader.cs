using Eventual.EventStore.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eventual.EventStore.Readers.Reactive
{
    public interface IEventStreamTrackedReactiveReader
    {
        Task CatchUpAllEventStreamsAsync(string trackerId, Action<Revision> onNext);

        Task CatchUpAllEventStreamsAsync(string trackerId, Func<Revision, Task> onNext);

        Task CatchUpAllEventStreamsAsync(string trackerId, Action<Revision> onNext, CancellationToken cancellationToken);

        Task CatchUpAllEventStreamsAsync(string trackerId, Func<Revision, Task> onNext, CancellationToken cancellationToken);

        Task ContinuouslyCatchUpAllEventStreamsAsync(string trackerId, Action<Revision> onNext);

        Task ContinuouslyCatchUpAllEventStreamsAsync(string trackerId, Func<Revision, Task> onNext);

        Task ContinuouslyCatchUpAllEventStreamsAsync(string trackerId, Action<Revision> onNext, CancellationToken cancellationToken);

        Task ContinuouslyCatchUpAllEventStreamsAsync(string trackerId, Func<Revision, Task> onNext, CancellationToken cancellationToken);
    }
}
