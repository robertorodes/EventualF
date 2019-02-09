using Eventual.EventStore.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eventual.EventStore.Readers.Reactive
{
    public interface IEventStreamReactiveReader
    {
        Task CatchUpAllEventStreamsAsync(GlobalCheckpoint initialCommit, Action<Revision> onNext);

        Task CatchUpAllEventStreamsAsync(GlobalCheckpoint initialCommit, Func<Revision, Task> onNext);

        Task CatchUpAllEventStreamsAsync(GlobalCheckpoint initialCommit, Action<Revision> onNext, CancellationToken cancellationToken);

        Task CatchUpAllEventStreamsAsync(GlobalCheckpoint initialCommit, Func<Revision, Task> onNext, CancellationToken cancellationToken);

        Task ContinuouslyCatchUpAllEventStreamsAsync(GlobalCheckpoint initialCommit, Action<Revision> onNext);

        Task ContinuouslyCatchUpAllEventStreamsAsync(GlobalCheckpoint initialCommit, Func<Revision, Task> onNext);

        Task ContinuouslyCatchUpAllEventStreamsAsync(GlobalCheckpoint initialCommit, Action<Revision> onNext, CancellationToken cancellationToken);

        Task ContinuouslyCatchUpAllEventStreamsAsync(GlobalCheckpoint initialCommit, Func<Revision, Task> onNext, CancellationToken cancellationToken);
    }
}
