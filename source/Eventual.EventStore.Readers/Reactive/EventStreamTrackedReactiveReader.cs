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
    public class EventStreamTrackedReactiveReader : IEventStreamTrackedReactiveReader
    {
        #region Attributes

        private IEventStreamTrackerRepository trackerRepository;
        private IEventStreamReader client;

        #endregion

        #region Constructors

        public EventStreamTrackedReactiveReader(IEventStreamTrackerRepository trackerRepository, IEventStreamReader client)
        {
            this.TrackerRepository = trackerRepository;
            this.Client = client;
        }

        #endregion

        #region Methods

        public async Task CatchUpAllEventStreamsAsync(string trackerId, Action<Revision> onNext)
        {
            // Set default cancellation token in case it is not provided (it will do nothing)
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            await CatchUpAllEventStreamsAsync(trackerId, onNext, cancellationToken);
        }

        public async Task CatchUpAllEventStreamsAsync(string trackerId, Action<Revision> onNext, CancellationToken cancellationToken)
        {
            await CatchUpAllEventStreams(trackerId, async state =>
            {
                // Execute onNext task specified by the client
                onNext(state.Revision);

                state.Tracker.UpdateCheckpoint(GlobalCheckpoint.Create(state.Revision.CommitId));
                await this.TrackerRepository.CommitChanges();
            },
            cancellationToken);
        }

        public async Task CatchUpAllEventStreamsAsync(string trackerId, Func<Revision, Task> onNext)
        {
            // Set default cancellation token in case it is not provided (it will do nothing)
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            await CatchUpAllEventStreamsAsync(trackerId, onNext, cancellationToken);
        }

        public async Task CatchUpAllEventStreamsAsync(string trackerId, Func<Revision, Task> onNext, CancellationToken cancellationToken)
        {
            await CatchUpAllEventStreams(trackerId, async state =>
            {
                // Execute onNext task specified by the client
                await onNext(state.Revision);

                state.Tracker.UpdateCheckpoint(GlobalCheckpoint.Create(state.Revision.CommitId));
                await this.TrackerRepository.CommitChanges();
            },
            cancellationToken);
        }

        private async Task CatchUpAllEventStreams(string trackerId, Func<EventStreamTrackingPipelineState, Task> handleRevision,
            CancellationToken cancellationToken)
        {
            try
            {
                // Subscribe to all event streams and execute onNext action and save changes for each revision received
                await (this.AllEventStreamsFromWithTracker(trackerId)
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

        private IObservable<EventStreamTrackingPipelineState> AllEventStreamsFromWithTracker(string trackerId)
        {
            if (trackerRepository == null)
            {
                throw new ArgumentNullException("trackerRepository");
            }

            return Observable.Create<EventStreamTrackingPipelineState>(async observer =>
            {
                var tracker = await this.GetTrackerOrCreate(trackerId);

                if (tracker.Type != CheckpointType.GlobalCheckpoint)
                {
                    throw new EventStoreClientInvalidTrackerCheckpointException();
                }

                return EventStoreObservables.AllEventStreamsFrom(this.Client, tracker.GlobalCheckpoint)
                    .Subscribe(revision =>
                    {
                        observer.OnNext(new EventStreamTrackingPipelineState
                        {
                            Tracker = tracker,
                            Revision = revision
                        });
                    },
                    observer.OnError,
                    observer.OnCompleted);
            });
        }

        public async Task ContinuouslyCatchUpAllEventStreamsAsync(string trackerId, Action<Revision> onNext)
        {
            // Set default cancellation token in case it is not provided (it will do nothing)
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            await ContinuouslyCatchUpAllEventStreamsAsync(trackerId, onNext, cancellationToken);
        }

        public async Task ContinuouslyCatchUpAllEventStreamsAsync(string trackerId, Action<Revision> onNext, CancellationToken cancellationToken)
        {
            //EventStreamTracker tracker = await GetTrackerOrCreate(trackerId);

            await ContinuouslyCatchUpAllEventStreams(trackerId, async (state) =>
            {
                // Execute onNext task specified by the client
                onNext(state.Revision);

                state.Tracker.UpdateCheckpoint(GlobalCheckpoint.Create(state.Revision.CommitId));
                await this.TrackerRepository.CommitChanges();
            },
            cancellationToken);
        }

        public async Task ContinuouslyCatchUpAllEventStreamsAsync(string trackerId, Func<Revision, Task> onNext)
        {
            // Set default cancellation token in case it is not provided (it will do nothing)
            CancellationToken cancellationToken = new CancellationTokenSource().Token;

            await ContinuouslyCatchUpAllEventStreamsAsync(trackerId, onNext, cancellationToken);
        }

        public async Task ContinuouslyCatchUpAllEventStreamsAsync(string trackerId, Func<Revision, Task> onNext, CancellationToken cancellationToken)
        {
            await ContinuouslyCatchUpAllEventStreams(trackerId, async (state) =>
            {
                // Execute onNext task specified by the client
                await onNext(state.Revision);

                state.Tracker.UpdateCheckpoint(GlobalCheckpoint.Create(state.Revision.CommitId));
                await this.TrackerRepository.CommitChanges();
            },
            cancellationToken);
        }

        private async Task ContinuouslyCatchUpAllEventStreams(string trackerId, Func<EventStreamTrackingPipelineState, Task> handleRevision,
            CancellationToken cancellationToken)
        {
            try
            {
                // Subscribe to all event streams and execute onNext action and save changes for each revision received
                await (this.ContinuousAllEventStreamsFromWithTracker(trackerId)
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

        private IObservable<EventStreamTrackingPipelineState> ContinuousAllEventStreamsFromWithTracker(string trackerId)
        {
            if (trackerRepository == null)
            {
                throw new ArgumentNullException("trackerRepository");
            }

            return Observable.Create<EventStreamTrackingPipelineState>(async observer =>
            {
                var tracker = await this.GetTrackerOrCreate(trackerId);

                if (tracker.Type != CheckpointType.GlobalCheckpoint)
                {
                    throw new EventStoreClientInvalidTrackerCheckpointException();
                }

                return EventStoreObservables.ContinuousAllEventStreamsFrom(this.Client, tracker.GlobalCheckpoint)
                    .Subscribe(revision =>
                    {
                        observer.OnNext(new EventStreamTrackingPipelineState
                        {
                            Tracker = tracker,
                            Revision = revision
                        });
                    },
                    observer.OnError,
                    observer.OnCompleted);
            });
        }

        private async Task<EventStreamTracker> GetTrackerOrCreate(string trackerId)
        {
            // Load last global checkpoint from tracker
            EventStreamTracker tracker = await this.TrackerRepository.GetEventStreamTracker(trackerId, true);

            // Create tracker if it does not exist
            if (tracker == null)
            {
                tracker = new EventStreamTracker(trackerId, GlobalCheckpoint.Create(0));
                await this.TrackerRepository.CreateEventStreamTracker(tracker);
            }

            return tracker;
        }

        #endregion

        #region Properties

        private IEventStreamTrackerRepository TrackerRepository
        {
            get
            {
                return trackerRepository;
            }
            set
            {
                //TODO: Insert validation code here
                trackerRepository = value;
            }
        }

        private IEventStreamReader Client
        {
            get
            {
                return this.client;
            }
            set
            {
                //TODO: Insert validation code here
                this.client = value;
            }
        }


        #endregion
    }
}
