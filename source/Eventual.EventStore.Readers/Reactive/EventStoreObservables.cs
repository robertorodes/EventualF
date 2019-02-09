using Eventual.EventStore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;

namespace Eventual.EventStore.Readers.Reactive
{
    public static class EventStoreObservables
    {
        private const int NumberOfResultsPerRequest = 2;
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(15);
        private const int DefaultRetries = 3;
        private static readonly TimeSpan DefaultPollingInterval = TimeSpan.FromSeconds(5);

        #region Individual event stream catch-up observables

        public static IObservable<Revision> EventStreamFrom(IEventStreamReader eventStoreClient, EventStreamCheckpoint checkpoint,
            int retries = DefaultRetries, IScheduler scheduler = null)
        {
            return EventStreamFrom(eventStoreClient, checkpoint, DefaultTimeout, retries, scheduler);
        }

        public static IObservable<Revision> EventStreamFrom(IEventStreamReader eventStoreClient, EventStreamCheckpoint checkpoint,
            TimeSpan timeout, int retries = DefaultRetries, IScheduler scheduler = null)
        {
            return Observable.Create<Revision>(observer =>
            {
                var poll = EventStreamBatchesFrom(eventStoreClient, checkpoint, timeout, retries, scheduler);

                return poll.Subscribe(
                    value =>
                    {
                        foreach (var revision in value)
                        {
                            observer.OnNext(revision);
                        }
                    },
                    ex => observer.OnError(ex),
                    () => observer.OnCompleted());
            });
        }

        public static IObservable<Revision> ContinuousEventStreamFrom(IEventStreamReader eventStoreClient, EventStreamCheckpoint checkpoint,
            int retries = DefaultRetries, IScheduler scheduler = null)
        {
            return ContinuousEventStreamFrom(eventStoreClient, checkpoint, DefaultTimeout, DefaultPollingInterval, retries, scheduler);
        }

        public static IObservable<Revision> ContinuousEventStreamFrom(IEventStreamReader eventStoreClient, EventStreamCheckpoint checkpoint,
            TimeSpan timeout, int retries = DefaultRetries, IScheduler scheduler = null)
        {
            return ContinuousEventStreamFrom(eventStoreClient, checkpoint, timeout, DefaultPollingInterval, retries, scheduler);
        }

        public static IObservable<Revision> ContinuousEventStreamFrom(IEventStreamReader eventStoreClient, EventStreamCheckpoint checkpoint,
            TimeSpan timeout, TimeSpan pollingInterval, int retries = DefaultRetries, IScheduler scheduler = null)
        {
            return Observable.Create<Revision>(observer =>
            {
                var poll = ContinuousEventStreamBatchesFrom(eventStoreClient, checkpoint, pollingInterval, timeout, retries, scheduler);

                return poll.Subscribe(
                    value =>
                    {
                        foreach (var revision in value)
                        {
                            observer.OnNext(revision);
                        }
                    },
                    ex => observer.OnError(ex),
                    () => observer.OnCompleted());
            });
        }

        #region Private observables (supporting catch-up individual event stream observables)

        private static IObservable<IEnumerable<Revision>> EventStreamBatchesFrom(IEventStreamReader eventStoreClient, EventStreamCheckpoint checkpoint,
            TimeSpan timeout, int retries = DefaultRetries, IScheduler scheduler = null)
        {
            return EventStreamBatchesFrom(eventStoreClient, checkpoint, timeout, TimeSpan.Zero, false, retries, scheduler);
        }

        private static IObservable<IEnumerable<Revision>> ContinuousEventStreamBatchesFrom(IEventStreamReader eventStoreClient, EventStreamCheckpoint checkpoint,
            TimeSpan timeout, TimeSpan pollingInterval, int retries = DefaultRetries, IScheduler scheduler = null)
        {
            return EventStreamBatchesFrom(eventStoreClient, checkpoint, timeout, pollingInterval, true, retries, scheduler);
        }

        private static IObservable<IEnumerable<Revision>> EventStreamBatchesFrom(IEventStreamReader eventStoreClient, EventStreamCheckpoint checkpoint,
            TimeSpan timeout, TimeSpan pollingInterval, bool infinitePollingEnabled = false, int retries = DefaultRetries, IScheduler scheduler = null)
        {
            scheduler = scheduler ?? Scheduler.Default;

            return ObservableExtensions.Generate(async () =>
            {
                IEnumerable<Revision> result = await EventStreamSingleBatch(eventStoreClient, checkpoint,
                    NumberOfResultsPerRequest, timeout, retries, scheduler);
                return new EventStreamPollingState
                {
                    LastCheckpoint = EventStreamCheckpoint.CreateStreamCheckpoint(checkpoint.StreamId, checkpoint.RevisionId + result.Count(), 0),
                    LastStreamBatch = result
                };
            },
                value => (value.LastStreamBatch != null && value.LastStreamBatch.Count() > 0) || infinitePollingEnabled,
                async value =>
                {
                    var streamBatch = EventStreamSingleBatch(eventStoreClient, value.LastCheckpoint,
                        NumberOfResultsPerRequest, timeout, retries, scheduler);

                    IEnumerable<Revision> result = null;

                    //Introduce delay before performing next request (if required)
                    var delay = Observable.Empty<IEnumerable<Revision>>().Delay(pollingInterval);
                    if (value.LastStreamBatch == null || value.LastStreamBatch.Count() == 0 && infinitePollingEnabled)
                    {
                        result = await delay.Concat(streamBatch);
                    }
                    else
                    {
                        result = await streamBatch;
                    }

                    if (result != null && result.Count() > 0)
                    {
                        return new EventStreamPollingState
                        {
                            LastCheckpoint = EventStreamCheckpoint.CreateStreamCheckpoint(value.LastCheckpoint.StreamId, value.LastCheckpoint.RevisionId + result.Count(), 0),
                            LastStreamBatch = result
                        };
                    }
                    else
                    {
                        return new EventStreamPollingState
                        {
                            LastCheckpoint = EventStreamCheckpoint.CreateStreamCheckpoint(value.LastCheckpoint.StreamId, value.LastCheckpoint.RevisionId, 0),
                            LastStreamBatch = Enumerable.Empty<Revision>()
                        };
                    }
                },
                value => value.LastStreamBatch, scheduler);
        }

        public static IObservable<IEnumerable<Revision>> EventStreamSingleBatch(IEventStreamReader eventStoreClient, EventStreamCheckpoint checkpoint,
            int numberOfResults, TimeSpan timeout, int retries, IScheduler scheduler)
        {
            return Observable.Return(1, scheduler)
                .SelectMany(_ =>
                {
                    return eventStoreClient.GetEventStreamFromAsync(checkpoint.StreamId, checkpoint.RevisionId, numberOfResults);
                })
                .Do(_ => Console.WriteLine(string.Format("Requested event stream range [{0} - {1}]", checkpoint.RevisionId, checkpoint.RevisionId + numberOfResults - 1)))
                .Timeout(timeout, scheduler)
                .Retry(retries)
                .Select(stream => stream.Revisions.AsEnumerable());
        }

        #endregion

        #endregion

        #region All event streams catch-up observables

        public static IObservable<Revision> AllEventStreamsFrom(IEventStreamReader eventStoreClient, GlobalCheckpoint checkpoint,
            int retries = DefaultRetries, IScheduler scheduler = null)
        {
            return AllEventStreamsFrom(eventStoreClient, checkpoint, DefaultTimeout, retries, scheduler);
        }

        public static IObservable<Revision> AllEventStreamsFrom(IEventStreamReader eventStoreClient, GlobalCheckpoint checkpoint,
            TimeSpan timeout, int retries = DefaultRetries, IScheduler scheduler = null)
        {
            return Observable.Create<Revision>(observer =>
            {
                var poll = AllEventStreamBatchesFrom(eventStoreClient, checkpoint, timeout, retries, scheduler);

                return poll.Subscribe(
                    value =>
                    {
                        foreach (var revision in value)
                        {
                            observer.OnNext(revision);
                        }
                    },
                    ex => observer.OnError(ex),
                    () => observer.OnCompleted());
            });
        }

        public static IObservable<Revision> ContinuousAllEventStreamsFrom(IEventStreamReader eventStoreClient, GlobalCheckpoint checkpoint,
            int retries = DefaultRetries, IScheduler scheduler = null)
        {
            return ContinuousAllEventStreamsFrom(eventStoreClient, checkpoint, DefaultTimeout, DefaultPollingInterval, retries, scheduler);
        }

        public static IObservable<Revision> ContinuousAllEventStreamsFrom(IEventStreamReader eventStoreClient, GlobalCheckpoint checkpoint,
            TimeSpan timeout, int retries = DefaultRetries, IScheduler scheduler = null)
        {
            return ContinuousAllEventStreamsFrom(eventStoreClient, checkpoint, timeout, DefaultPollingInterval, retries, scheduler);
        }

        public static IObservable<Revision> ContinuousAllEventStreamsFrom(IEventStreamReader eventStoreClient, GlobalCheckpoint checkpoint,
            TimeSpan timeout, TimeSpan pollingInterval, int retries = DefaultRetries, IScheduler scheduler = null)
        {
            return Observable.Create<Revision>(observer =>
            {
                var poll = ContinuousAllEventStreamBatchesFrom(eventStoreClient, checkpoint, timeout, pollingInterval, retries, scheduler);

                return poll.Subscribe(
                    value =>
                    {
                        foreach (var revision in value)
                        {
                            observer.OnNext(revision);
                        }
                    },
                    ex => observer.OnError(ex),
                    () => observer.OnCompleted());
            });
        }

        #region Private observables (supporting catch-up all event streams observables)

        private static IObservable<IEnumerable<Revision>> AllEventStreamBatchesFrom(IEventStreamReader eventStoreClient, GlobalCheckpoint checkpoint, TimeSpan timeout,
            int retries, IScheduler scheduler)
        {
            return AllEventStreamBatchesFrom(eventStoreClient, checkpoint, timeout, TimeSpan.Zero, false, retries, scheduler);
        }

        private static IObservable<IEnumerable<Revision>> ContinuousAllEventStreamBatchesFrom(IEventStreamReader eventStoreClient, GlobalCheckpoint checkpoint,
            TimeSpan timeout, TimeSpan pollingInterval, int retries = DefaultRetries, IScheduler scheduler = null)
        {
            return AllEventStreamBatchesFrom(eventStoreClient, checkpoint, timeout, pollingInterval, true, retries, scheduler);
        }

        private static IObservable<IEnumerable<Revision>> AllEventStreamBatchesFrom(IEventStreamReader eventStoreClient, GlobalCheckpoint checkpoint, TimeSpan timeout,
            TimeSpan pollingInterval, bool infinitePollingEnabled = false, int retries = DefaultRetries, IScheduler scheduler = null)
        {
            scheduler = scheduler ?? Scheduler.Default;

            return ObservableExtensions.Generate(async () =>
            {
                IEnumerable<Revision> result = await AllEventStreamsSingleBatch(eventStoreClient, checkpoint,
                    NumberOfResultsPerRequest, timeout, retries, scheduler);
                return new AllEventStreamsPollingState
                {
                    LastCheckpoint = GlobalCheckpoint.Create(checkpoint.CommitId + result.Count()),
                    LastStreamBatch = result
                };
            },
                value => (value.LastStreamBatch != null && value.LastStreamBatch.Count() > 0) || infinitePollingEnabled,
                async value =>
                {
                    var streamBatch = AllEventStreamsSingleBatch(eventStoreClient, value.LastCheckpoint,
                        NumberOfResultsPerRequest, timeout, retries, scheduler);

                    IEnumerable<Revision> result = null;

                    //Introduce delay before performing next request (if required)
                    var delay = Observable.Empty<IEnumerable<Revision>>().Delay(pollingInterval);
                    if (value.LastStreamBatch == null || value.LastStreamBatch.Count() == 0 && infinitePollingEnabled)
                    {
                        result = await delay.Concat(streamBatch);
                    }
                    else
                    {
                        result = await streamBatch;
                    }

                    if (result != null && result.Count() > 0)
                    {
                        return new AllEventStreamsPollingState
                        {
                            LastCheckpoint = GlobalCheckpoint.Create(value.LastCheckpoint.CommitId + result.Count()),
                            LastStreamBatch = result
                        };
                    }
                    else
                    {
                        return new AllEventStreamsPollingState
                        {
                            LastCheckpoint = GlobalCheckpoint.Create(value.LastCheckpoint.CommitId),
                            LastStreamBatch = Enumerable.Empty<Revision>()
                        };
                    }
                },
                value => value.LastStreamBatch, scheduler);
        }

        private static IObservable<IEnumerable<Revision>> AllEventStreamsSingleBatch(IEventStreamReader eventStoreClient,
            GlobalCheckpoint checkpoint, int numberOfResults, TimeSpan timeout, int retries, IScheduler scheduler)
        {
            return Observable.Return(1, scheduler)
                .SelectMany(_ =>
                {
                    return eventStoreClient.GetAllEventStreamsFromAsync(checkpoint.CommitId, numberOfResults);
                })
                .Do(_ => Console.WriteLine(string.Format("Requested all event streams range (commit) [{0} - {1}]", checkpoint.CommitId, checkpoint.CommitId + numberOfResults - 1)))
                .Timeout(timeout, scheduler)
                .Retry(retries)
                .Select(stream => stream.Revisions.AsEnumerable());
        }

        #endregion

        #endregion
    }
}
