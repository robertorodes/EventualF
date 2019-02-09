using Eventual.EventStore.Core;
using Eventual.EventStore.EntityFrameworkCore;
using Eventual.EventStore.Readers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventStreamReaderExample
{
    class Program
    {
        static void Main(string[] args)
        {
            AppInitializer.Initialize();

            var options = new DbContextOptionsBuilder<EventStoreDbContext>()
                .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EventStreamReaderExample;Integrated Security=False")
                .Options;
            var dbContext = new EventStoreDbContext(options);
            IEventStore eventStore = new EFEventStore(dbContext, null);
            IEventStreamReader eventStreamReader = new InProcessEventStreamReader(eventStore);


            Console.WriteLine("CATCH UP EVENT STREAM");
            Task<EventStream> task = null;
            int initialRevision = 0;
            int batchSize = 1;
            do
            {
                task = eventStreamReader.GetEventStreamFromAsync("19e2a7fc-d0eb-44b9-8348-e7678ccd37bc", initialRevision, batchSize);
                initialRevision += batchSize;
                task.Wait();

                foreach (var revision in task.Result.Revisions)
                {
                    Console.WriteLine("Aggregate: " + revision.AggregateType + " - Revision: " + revision.RevisionId);
                }

            } while (task.Result != null && task.Result.Revisions.Count > 0);

            Console.WriteLine("Completed (up to date)");

            Console.WriteLine();
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
            Console.WriteLine();
        }
    }
}
