using Eventual.EventStore.Core;
using Eventual.EventStore.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eventual.EventStore.Readers.Tests.Integration
{
    static class DatabaseSeeder
    {
        static public void Seed(EventStoreDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            // Aggreggate 1
            string aggregate01Id = "19e2a7fc-d0eb-44b9-8348-e7678ccd37bc";
            var seedRevision = GetSeedRevision(aggregate01Id, "Test aggregate 1", 0);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate01Id, "Test aggregate 1", 1);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate01Id, "Test aggregate 1", 2);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate01Id, "Test aggregate 1", 3);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate01Id, "Test aggregate 1", 4);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate01Id, "Test aggregate 1", 5);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate01Id, "Test aggregate 1", 6);
            dbContext.StoredRevisions.Add(seedRevision);

            // Aggregate 2
            string aggregate02Id = "f552b2d2-4b30-4447-b187-14cc8b572379";
            seedRevision = GetSeedRevision(aggregate02Id, "Test aggregate 2", 0);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate02Id, "Test aggregate 2", 1);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate02Id, "Test aggregate 2", 2);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate02Id, "Test aggregate 2", 3);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate02Id, "Test aggregate 2", 4);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate02Id, "Test aggregate 2", 5);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate02Id, "Test aggregate 2", 6);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate02Id, "Test aggregate 2", 7);
            dbContext.StoredRevisions.Add(seedRevision);

            // Aggregate 3
            string aggregate03Id = "1f02431f-5716-4189-bd5b-529ed837503c";
            seedRevision = GetSeedRevision(aggregate03Id, "Test aggregate 3", 0);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate03Id, "Test aggregate 3", 1);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate03Id, "Test aggregate 3", 2);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate03Id, "Test aggregate 3", 3);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate03Id, "Test aggregate 3", 4);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate03Id, "Test aggregate 3", 5);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate03Id, "Test aggregate 3", 6);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate03Id, "Test aggregate 3", 7);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate03Id, "Test aggregate 3", 8);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate03Id, "Test aggregate 3", 9);
            dbContext.StoredRevisions.Add(seedRevision);

            seedRevision = GetSeedRevision(aggregate03Id, "Test aggregate 3", 10);
            dbContext.StoredRevisions.Add(seedRevision);

            // Aggregate 4
            string aggregate04Id = "cc09d9d1-62fa-432c-aa21-98b70eb4cff1";
            seedRevision = GetSeedRevision(aggregate04Id, "Test aggregate 4", 0);
            dbContext.StoredRevisions.Add(seedRevision);

            dbContext.SaveChanges();
        }

        static private Revision GetSeedRevision(string aggregateId, string aggregateType, int revisionId)
        {
            return new Revision()
            {
                AggregateId = new Guid(aggregateId),
                AggregateType = aggregateType,
                CausationId = "Test command",
                Changes = Encoding.UTF8.GetBytes("These are some fake events."),
                ChangesContentEncoding = Encoding.UTF8.HeaderName,
                ChangesContentType = "text/plain",
                CorrelationId = "Test message",
                Metadata = Encoding.UTF8.GetBytes("This is some fake metadata."),
                MetadataContentEncoding = Encoding.UTF8.HeaderName,
                MetadataContentType = "text/plain",
                OccurrenceDate = DateTime.UtcNow,
                RevisionId = revisionId
            };
        }
    }
}
