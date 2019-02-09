using Eventual.EventStore.Readers;
using Eventual.EventStore.Readers.Tracking.Storage.EntityFramework;
using Eventual.EventStore.Core;
using Eventual.EventStore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Eventual.EventStore.Readers.Reactive;
using Eventual.EventStore.Readers.Tracking;

namespace EventStreamReactiveObservablesExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = null;
            CancellationTokenSource ct = null;

            AppInitializer.Initialize();

            var options = new DbContextOptionsBuilder<EventStoreDbContext>()
                .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EventStreamReactiveObservablesExample;Integrated Security=False")
                .Options;
            var dbContext = new EventStoreDbContext(options);
            IEventStore eventStore = new EFEventStore(dbContext, null);
            IEventStreamReader eventStreamReader = new InProcessEventStreamReader(eventStore);


            Console.WriteLine("CATCHING UP EVENT STREAM");
            t = EventStoreObservables.EventStreamFrom(eventStreamReader, EventStreamCheckpoint.CreateStreamCheckpoint("19e2a7fc-d0eb-44b9-8348-e7678ccd37bc", 0, 0))
                .ForEachAsync(revision => Console.WriteLine("--> Aggregate: " + revision.AggregateType + " - Revision: " + revision.RevisionId));

            t.Wait();
            Console.WriteLine("Completed (up to date)");

            Console.WriteLine();
            Console.WriteLine("Press <ENTER> to continue...");
            Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("CONTINUOUSLY CATCHING UP EVENT STREAM");
            ct = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            try
            {
                t = EventStoreObservables.ContinuousEventStreamFrom(eventStreamReader, EventStreamCheckpoint.CreateStreamCheckpoint("19e2a7fc-d0eb-44b9-8348-e7678ccd37bc", 0, 0))
                .ForEachAsync(revision => Console.WriteLine("--> Aggregate: " + revision.AggregateType + " - Revision: " + revision.RevisionId), ct.Token);
                t.Wait();
            }
            catch (AggregateException e)
            {
                if (e.InnerException is TaskCanceledException)
                {
                    Console.WriteLine("ERROR: The operation has been canceled => " + e.InnerException.Message);
                }
                else
                {
                    throw e;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press <ENTER> to continue...");
            Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("CATCHING UP ALL EVENT STREAMS");
            t = EventStoreObservables.AllEventStreamsFrom(eventStreamReader, GlobalCheckpoint.Create(27))
                .ForEachAsync(revision => Console.WriteLine("--> Aggregate: " + revision.AggregateType + " - Revision: " + revision.RevisionId));

            t.Wait();
            Console.WriteLine("Completed (up to date)");

            Console.WriteLine();
            Console.WriteLine("Press <ENTER> to continue...");
            Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("CONTINUOUSLY CATCHING UP ALL EVENT STREAMS");
            ct = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            try
            {
                t = EventStoreObservables.ContinuousAllEventStreamsFrom(eventStreamReader, GlobalCheckpoint.Create(1))
                .ForEachAsync(revision => Console.WriteLine("--> Aggregate: " + revision.AggregateType + " - Revision: " + revision.RevisionId), ct.Token);
                t.Wait();
            }
            catch (AggregateException e)
            {
                if (e.InnerException is TaskCanceledException)
                {
                    Console.WriteLine("ERROR: The operation has been canceled => " + e.Message);
                }
                else
                {
                    throw e;
                }
            }

            // Dispose resources
            dbContext.Dispose();

            Console.WriteLine();
            Console.WriteLine("Press <ENTER> to exit...");
            Console.ReadLine();
        }
    }
}
