﻿using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eventual.EventStore.Readers.Reactive
{
    static class ObservableExtensions
    {
        public static IObservable<TResult> Generate<TState, TResult>(
            Func<Task<TState>> initialState,
            Func<TState, bool> condition,
            Func<TState, Task<TState>> iterate,
            Func<TState, TResult> resultSelector,
            IScheduler scheduler = null)
        {
            var s = scheduler ?? Scheduler.Default;

            return Observable.Create<TResult>(async obs =>
            {
                //var initialTask = initialState();
                //initialTask.Wait();
                return s.Schedule(await initialState(), async (state, self) =>
                //return s.Schedule(initialTask.Result, async (state, self) =>
                {
                    if (!condition(state))
                    {
                        obs.OnCompleted();
                        return;
                    }

                    obs.OnNext(resultSelector(state));

                    try
                    {
                        self(await iterate(state));
                        //var task = iterate(state);
                        //task.Wait();
                        //self(task.Result);
                    }
                    catch (Exception e)
                    {
                        obs.OnError(e);
                    }
                });
            });
        }

        // The following select is a workaround to allow calling an asynchronous function from within the observer in a non-blocking way
        // (see http://stackoverflow.com/questions/35184053/how-to-implement-an-iobserver-with-async-await-onnext-onerror-oncompleted-method)
        public static IObservable<TResult> SelectFromAsync<TSource, TResult>(this IObservable<TSource> source, Func<TSource, Task<TResult>> selector)
        {
            return source.Select(revision => Observable.Defer<TResult>(() =>
            {
                return selector(revision).ToObservable();
            }))
                .Concat();
        }

        // The following select is a workaround to allow calling an asynchronous function from within the observer in a non-blocking way
        // (see http://stackoverflow.com/questions/35184053/how-to-implement-an-iobserver-with-async-await-onnext-onerror-oncompleted-method)
        public static IObservable<Unit> SelectFromAsync<TSource>(this IObservable<TSource> source, Func<TSource, Task> selector)
        {
            return source.Select(revision => Observable.Defer(() =>
            {
                return selector(revision).ToObservable();
            }))
                .Concat();
        }

        public static IDisposable SubscribeAsync<T>(this IObservable<T> source, Func<T, Task> onNext, Action<Exception> onError, Action onCompleted)
        {
            return source.Select(e => Observable.Defer(() => onNext(e).ToObservable())).Concat()
                .Subscribe(
                e => { }, // empty
                onError,
                onCompleted);
        }

        public static void SubscribeAsync<T>(this IObservable<T> source, Func<T, Task> onNext, CancellationToken cancellationToken)
        {
            source.Select(e => Observable.Defer(() => onNext(e).ToObservable())).Concat()
                .Subscribe(
                e => { }, // empty
                cancellationToken);
        }

        /// <summary>
        /// An exponential back off strategy which starts with 1 second and then 4, 9, 16...
        /// </summary>
        //[SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly Func<int, TimeSpan> ExponentialBackoff = n => TimeSpan.FromSeconds(Math.Pow(n, 2));

        /// <summary>
        /// Returns a cold observable which retries (re-subscribes to) the source observable on error up to the 
        /// specified number of times or until it successfully terminates. Allows for customizable back off strategy.
        /// </summary>
        /// <param name="source">The source observable.</param>
        /// <param name="retryCount">The number of attempts of running the source observable before failing.</param>
        /// <param name="strategy">The strategy to use in backing off, exponential by default.</param>
        /// <param name="retryOnError">A predicate determining for which exceptions to retry. Defaults to all</param>
        /// <param name="scheduler">The scheduler.</param>
        /// <returns>
        /// A cold observable which retries (re-subscribes to) the source observable on error up to the 
        /// specified number of times or until it successfully terminates.
        /// </returns>
        //[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static IObservable<T> RetryWithBackoffStrategy<T>(
            this IObservable<T> source,
            int retryCount = 3,
            Func<int, TimeSpan> strategy = null,
            Func<Exception, bool> retryOnError = null,
            IScheduler scheduler = null)
        {
            strategy = strategy ?? ExponentialBackoff;
            scheduler = scheduler ?? Scheduler.Default;

            if (retryOnError == null)
                retryOnError = e => true;

            int attempt = 0;

            return Observable.Defer(() =>
            {
                return ((++attempt == 1) ? source : source.DelaySubscription(strategy(attempt - 1), scheduler))
                    .Select(item => new Tuple<bool, T, Exception>(true, item, null))
                    .Catch<Tuple<bool, T, Exception>, Exception>(e => retryOnError(e)
                        ? Observable.Throw<Tuple<bool, T, Exception>>(e)
                        : Observable.Return(new Tuple<bool, T, Exception>(false, default(T), e)));
            })
            .Retry(retryCount)
            .SelectMany(t => t.Item1
                ? Observable.Return(t.Item2)
                : Observable.Throw<T>(t.Item3));
        }
    }
}
