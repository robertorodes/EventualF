using Eventual.EventStore.Core;
using Eventual.EventStore.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventStreamReactiveReaderExample
{
    static class DatabaseSeeder
    {
        static public void Seed(EventStoreDbContext context)
        {
            var seedRevision = new Revision()
            {
                AggregateId = new Guid("19e2a7fc-d0eb-44b9-8348-e7678ccd37bc"),
                AggregateType = "Test aggregate 1",
                CausationId = "Test command",
                Changes = Encoding.UTF8.GetBytes("These are some fake events."),
                ChangesContentEncoding = Encoding.UTF8.HeaderName,
                ChangesContentType = "text/plain",
                CorrelationId = "Test message",
                Metadata = Encoding.UTF8.GetBytes("This is some fake metadata."),
                MetadataContentEncoding = Encoding.UTF8.HeaderName,
                MetadataContentType = "text/plain",
                OccurrenceDate = DateTime.UtcNow,
                RevisionId = 0,
                CommitId = default(long)
            };

            // Aggreggate 1
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 1;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 2;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 3;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 4;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 5;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 6;
            context.StoredRevisions.Add(seedRevision);

            // Aggregate 2
            seedRevision = CreateCopy(seedRevision);
            seedRevision.AggregateId = new Guid("f552b2d2-4b30-4447-b187-14cc8b572379");
            seedRevision.AggregateType = "Test aggregate 2";
            seedRevision.RevisionId = 0;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 1;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 2;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 3;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 4;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 5;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 6;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 7;
            context.StoredRevisions.Add(seedRevision);

            // Aggregate 3
            seedRevision = CreateCopy(seedRevision);
            seedRevision.AggregateId = new Guid("1f02431f-5716-4189-bd5b-529ed837503c");
            seedRevision.AggregateType = "Test aggregate 3";
            seedRevision.RevisionId = 0;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 1;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 2;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 3;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 4;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 5;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 6;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 7;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 8;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 9;
            context.StoredRevisions.Add(seedRevision);

            seedRevision = CreateCopy(seedRevision);
            seedRevision.RevisionId = 10;
            context.StoredRevisions.Add(seedRevision);

            // Aggregate 4
            seedRevision = CreateCopy(seedRevision);
            seedRevision.AggregateId = new Guid("cc09d9d1-62fa-432c-aa21-98b70eb4cff1");
            seedRevision.AggregateType = "Test aggregate 4";
            seedRevision.RevisionId = 0;
            context.StoredRevisions.Add(seedRevision);

            context.SaveChanges();
        }

        static private Revision CreateCopy(Revision source)
        {
            return new Revision()
            {
                AggregateId = source.AggregateId,
                AggregateType = source.AggregateType,
                CausationId = source.CausationId,
                Changes = source.Changes,
                ChangesContentEncoding = source.ChangesContentEncoding,
                ChangesContentType = source.ChangesContentType,
                CommitId = default(long),
                CorrelationId = source.CorrelationId,
                Metadata = source.Metadata,
                MetadataContentEncoding = source.MetadataContentEncoding,
                MetadataContentType = source.MetadataContentType,
                OccurrenceDate = source.OccurrenceDate,
                RevisionId = source.RevisionId
            };
        }
    }
}
