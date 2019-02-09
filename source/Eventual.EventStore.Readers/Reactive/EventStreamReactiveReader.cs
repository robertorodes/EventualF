using Eventual.EventStore.Core;
using Eventual.EventStore.Readers.Tracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eventual.EventStore.Readers.Reactive
{
    public class EventStreamReactiveReader : IEventStreamReactiveReader
    {
        #region Attributes

        private IEventStreamReader eventStreamReader;

        #endregion

        #region Constructors

        public EventStreamReactiveReader(IEventStreamReader eventStreamReader)
        {
            this.EventStreamReader = eventStreamReader;
        }

        #endregion

        #region Public methods

        public async Task CatchUpAllEventStreamsAsync(GlobalCheckpoint initialCommit, Action<Revision> onNext)
        {
            // Set default cancellation token in case it is not provided (it will do nothing)
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            await CatchUpAllEventStreamsAsync(initialCommit, onNext, cancellationToken);
        }

        public async Task CatchUpAllEventStreamsAsync(GlobalCheckpoint initialCommit, Func<Revision, Task> onNext)
        {
            // Set default cancellation token in case it is not provided (it will do nothing)
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            await CatchUpAllEventStreamsAsync(initialCommit, onNext, cancellationToken);
        }

        public async Task CatchUpAllEventStreamsAsync(GlobalCheckpoint initialCommit, Action<Revision> onNext, CancellationToken cancellationToken)
        {
            await CatchUpAllEventStreams(initialCommit, async revision =>
            {
                // Execute onNext task specified by the client
                onNext(revision);

                await Task.CompletedTask;
            },
            cancellationToken);
        }

        public async Task CatchUpAllEventStreamsAsync(GlobalCheckpoint initialCommit, Func<Revision, Task> onNext, CancellationToken cancellationToken)
        {
            await CatchUpAllEventStreams(initialCommit, onNext, cancellationToken);
        }

        public async Task ContinuouslyCatchUpAllEventStreamsAsync(GlobalCheckpoint initialCommit, Action<Revision> onNext)
        {
            // Set default cancellation token in case it is not provided (it will do nothing)
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            await ContinuouslyCatchUpAllEventStreamsAsync(initialCommit, onNext, cancellationToken);
        }

        public async Task ContinuouslyCatchUpAllEventStreamsAsync(GlobalCheckpoint initialCommit, Func<Revision, Task> onNext)
        {
            // Set default cancellation token in case it is not provided (it will do nothing)
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            await ContinuouslyCatchUpAllEventStreamsAsync(initialCommit, onNext, cancellationToken);
        }

        public async Task ContinuouslyCatchUpAllEventStreamsAsync(GlobalCheckpoint initialCommit, Action<Revision> onNext, CancellationToken cancellationToken)
        {
            await ContinuouslyCatchUpAllEventStreams(initialCommit, async revision =>
            {
                // Execute onNext task specified by the client
                onNext(revision);

                await Task.CompletedTask;
            },
            cancellationToken);
        }

        public async Task ContinuouslyCatchUpAllEventStreamsAsync(GlobalCheckpoint initialCommit, Func<Revision, Task> onNext, CancellationToken cancellationToken)
        {
            await ContinuouslyCatchUpAllEventStreams(initialCommit, onNext, cancellationToken);
        }

        #endregion

        #region Private methods

        private async Task CatchUpAllEventStreams(GlobalCheckpoint initialCommit, Func<Revision, Task> handleRevision, CancellationToken cancellationToken)
        {
            try
            {
                // Subscribe to all event streams and execute onNext action and save changes for each revision received
                await (EventStoreObservables.AllEventStreamsFrom(this.EventStreamReader, initialCommit)
                    .SelectFromAsync(handleRevision)
                    .RetryWithBackoffStrategy(retryCount: int.MaxValue, retryOnError: e => e is DbUpdateConcurrencyException)
                    .ToTask(cancellationToken));
            }
            catch (InvalidOperationException ex)
            {
                if (ex.HResult == -2146233079)
                {
                    // TODO: Write to log instead of the console
                    Console.WriteLine(string.Format("An invalid operation exception has been thrown with code {0} when processing results from the server. Perhaps you are already up to date. Detailed error: {1}", ex.HResult, ex.Message));
                }
                else
                {
                    throw ex;
                }
            }
        }

        private async Task ContinuouslyCatchUpAllEventStreams(GlobalCheckpoint initialCommit, Func<Revision, Task> handleRevision,
            CancellationToken cancellationToken)
        {
            try
            {
                // Subscribe to all event streams and execute onNext action and save changes for each revision received
                await (EventStoreObservables.ContinuousAllEventStreamsFrom(this.EventStreamReader, initialCommit)
                    .SelectFromAsync(handleRevision)
                    .RetryWithBackoffStrategy(retryCount: int.MaxValue, retryOnError: e => e is EventStreamTrackedReaderDbConcurrencyException)
                    .ToTask(cancellationToken));
            }
            catch (InvalidOperationException ex)
            {
                if (ex.HResult == -2146233079)
                {
                    // TODO: Write to log instead of the console
                    Console.WriteLine(string.Format("An invalid operation exception has been thrown with code {0} when processing results from the server. Perhaps you are already up to date. Detailed error: {1}", ex.HResult, ex.Message));
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region Properties

        private IEventStreamReader EventStreamReader
        {
            get
            {
                return this.eventStreamReader;
            }
            set
            {
                //TODO: Insert validation code here
                this.eventStreamReader = value;
            }
        }


        #endregion
    }
}
