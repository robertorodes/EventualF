using Eventual.EventStore.Core;
using Eventual.EventStore.EntityFrameworkCore;
using Eventual.EventStore.Readers;
using Eventual.EventStore.Readers.Reactive;
using Eventual.EventStore.Readers.Tracking;
using Eventual.EventStore.Readers.Tracking.Storage.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventStreamedTrackedReactiveReaderExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = null;
            CancellationTokenSource ct = null;

            AppInitializer.Initialize();

            var options = new DbContextOptionsBuilder<EventStoreDbContext>()
                .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EventStreamedTrackedReactiveReaderExample;Integrated Security=False")
                .Options;
            var dbContext = new EventStoreDbContext(options);
            IEventStore eventStore = new EFEventStore(dbContext, null);
            IEventStreamReader eventStreamReader = new InProcessEventStreamReader(eventStore);

            var eventStreamTrackingDbContext = new EventStreamTrackerDbContext(
                new DbContextOptionsBuilder<EventStreamTrackerDbContext>()
                .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EventStreamedTrackedReactiveReaderExampleTrackingDb;Integrated Security=False")
                .Options
                );
            IEventStreamTrackerRepository trackerRepository = new EFEventStreamTrackerRepository(eventStreamTrackingDbContext);
            IEventStreamTrackedReactiveReader eventStreamTrackedReader = new EventStreamTrackedReactiveReader(trackerRepository, eventStreamReader);

            Console.WriteLine();
            Console.WriteLine("CATCHING UP ALL EVENT STREAMS (WITH PERSISTENT TRACKING AND SYNCHRONOUS HANDLER)");

            try
            {
                t = eventStreamTrackedReader.CatchUpAllEventStreamsAsync("projection01", revision =>
                {
                    Console.WriteLine(string.Format("--> Processed revision: {0}", revision.ToString()));
                });
                t.Wait();
            }
            catch (AggregateException e)
            {
                Console.WriteLine(string.Format("ERROR: {0}", e.InnerException.Message));
            }


            Console.WriteLine();
            Console.WriteLine("Press <ENTER> to continue...");
            Console.ReadLine();
            Console.WriteLine();


            Console.WriteLine();
            Console.WriteLine("CONTINUOUSLY CATCHING UP ALL EVENT STREAMS (WITH PERSISTENT TRACKING AND SYNCHRONOUS HANDLER)");
            ct = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            try
            {
                t = eventStreamTrackedReader.ContinuouslyCatchUpAllEventStreamsAsync("projection02", revision =>
                {
                    Console.WriteLine(string.Format("--> Processed revision: {0}", revision.ToString()));
                }, ct.Token);
                t.Wait();
            }
            catch (AggregateException e)
            {
                Console.WriteLine(string.Format("ERROR: {0}", e.InnerException.Message));
            }



            Console.WriteLine();
            Console.WriteLine("CATCHING UP ALL EVENT STREAMS (WITH PERSISTENT TRACKING AND ASYNCHRONOUS HANDLER)");

            try
            {
                t = eventStreamTrackedReader.CatchUpAllEventStreamsAsync("projection03", async revision =>
                {
                    Console.WriteLine(string.Format("--> Processed revision: {0}", revision.ToString()));
                    await Task.FromResult(true);
                });
                t.Wait();
            }
            catch (AggregateException e)
            {
                Console.WriteLine(string.Format("ERROR: {0}", e.InnerException.Message));
            }

            Console.WriteLine();
            Console.WriteLine("Press <ENTER> to continue...");
            Console.ReadLine();
            Console.WriteLine();




            Console.WriteLine();
            Console.WriteLine("CONTINUOUSLY CATCHING UP ALL EVENT STREAMS (WITH PERSISTENT TRACKING AND ASYNCHRONOUS HANDLER)");
            ct = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            try
            {
                t = eventStreamTrackedReader.ContinuouslyCatchUpAllEventStreamsAsync("projection04", async revision =>
                {
                    Console.WriteLine(string.Format("--> Processed revision: {0}", revision.ToString()));
                    await Task.FromResult(true);
                }, ct.Token);
                t.Wait();
            }
            catch (AggregateException e)
            {
                Console.WriteLine(string.Format("ERROR: {0}", e.InnerException.Message));
            }

            // Dispose resources
            dbContext.Dispose();
            eventStreamTrackingDbContext.Dispose();

            Console.WriteLine();
            Console.WriteLine("Press <ENTER> to exit...");
            Console.ReadLine();
        }
    }
}
