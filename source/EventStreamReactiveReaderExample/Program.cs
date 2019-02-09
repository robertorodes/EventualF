using Eventual.EventStore.Core;
using Eventual.EventStore.EntityFrameworkCore;
using Eventual.EventStore.Readers;
using Eventual.EventStore.Readers.Reactive;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventStreamReactiveReaderExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = null;
            CancellationTokenSource ct = null;

            AppInitializer.Initialize();

            var options = new DbContextOptionsBuilder<EventStoreDbContext>()
                .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EventStreamReactiveReaderExample;Integrated Security=False")
                .Options;
            var dbContext = new EventStoreDbContext(options);
            IEventStore eventStore = new EFEventStore(dbContext, null);
            IEventStreamReader eventStreamReader = new InProcessEventStreamReader(eventStore);
            IEventStreamReactiveReader reactiveReader = new EventStreamReactiveReader(eventStreamReader);


            Console.WriteLine("******** REACTIVE READER EXAMPLES ********");
            Console.WriteLine();
            Console.WriteLine("CATCHING UP ALL EVENT STREAMS");

            try
            {
                t = reactiveReader.CatchUpAllEventStreamsAsync(GlobalCheckpoint.CreateFromStart(), revision =>
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
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
            Console.WriteLine();


            Console.WriteLine();
            Console.WriteLine("CONTINUOUSLY CATCHING UP ALL EVENT STREAMS");
            ct = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            try
            {
                t = reactiveReader.ContinuouslyCatchUpAllEventStreamsAsync(GlobalCheckpoint.CreateFromStart(), revision =>
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
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
            Console.WriteLine("CATCHING UP ALL EVENT STREAMS (WITH ASYNCHRONOUS HANDLER)");

            try
            {
                t = reactiveReader.CatchUpAllEventStreamsAsync(GlobalCheckpoint.CreateFromStart(), async revision =>
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
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine("CONTINUOUSLY CATCHING UP ALL EVENT STREAMS (WITH ASYNCHRONOUS HANDLER)");
            ct = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            try
            {
                t = reactiveReader.ContinuouslyCatchUpAllEventStreamsAsync(GlobalCheckpoint.CreateFromStart(), async revision =>
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

            Console.WriteLine();
            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }
    }
}
