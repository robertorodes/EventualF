using Eventual.Common.Logging;
using Eventual.EventStore.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventual.EventStore.EntityFrameworkCore
{
    public class EFEventStore : IEventStore
    {
        #region Attributes

        private IEventStoreDbContext dbContext;
        private ILogger logger;

        #endregion

        #region Constructors

        //TODO: Add dependencies
        public EFEventStore(EventStoreDbContext dbContext, ILogger logger)
        {
            this.DbContext = dbContext;
            this.Logger = logger;
        }

        #endregion

        #region Methods

        public async Task CommitChangesAsync(EventStreamCommit commit)
        {
            Revision storedRevision = new Revision()
            {
                AggregateId = commit.AggregateId,
                AggregateType = commit.AggregateType.ToString(),
                CorrelationId = commit.CorrelationId,
                CausationId = commit.CausationId,
                MetadataContentType = commit.MetadataContentType,
                MetadataContentEncoding = commit.MetadataContentEncoding,
                Metadata = commit.Metadata,
                ChangesContentType = commit.ChangesContentType,
                ChangesContentEncoding = commit.ChangesContentEncoding,
                Changes = commit.Changes,
                OccurrenceDate = DateTime.UtcNow,
                RevisionId = (commit.WorkingCopyVersion < 0) ? 0 : commit.WorkingCopyVersion + 1
            };

            DbContext.StoredRevisions.Add(storedRevision);

            await Task.CompletedTask;
        }

        public async Task<IList<Revision>> GetEventStreamAsync(Guid aggregateId)
        {
            var revisions = await DbContext.StoredRevisions
                .Where(t => t.AggregateId == aggregateId)
                .OrderBy(t => t.RevisionId)
                .ToListAsync();

            return revisions;
        }

        public async Task<IList<Revision>> GetEventStreamFromAsync(Guid aggregateId, int initialRevisionExclusive, int numberOfResults)
        {
            if (numberOfResults <= 0)
            {
                throw new ArgumentException("The number of results to retrieve must be higher than 0. Please, make sure to specify a positive value.",
                    "numberOfResults");
            }

            initialRevisionExclusive = (initialRevisionExclusive < -1) ? -1 : initialRevisionExclusive;

            var revisions = await DbContext.StoredRevisions
                .Where(t => t.AggregateId == aggregateId)
                .OrderBy(t => t.RevisionId)
                .Skip(initialRevisionExclusive + 1)
                .Take(numberOfResults)
                .ToListAsync();

            return revisions;
        }

        public async Task<IList<Revision>> GetAllEventStreamsFromAsync(long initialCommitExclusive, int numberOfResults)
        {
            if (numberOfResults <= 0)
            {
                throw new ArgumentException("The number of results to retrieve must be higher than 0. Please, make sure to specify a positive value.",
                    "numberOfResults");
            }

            // Here we are forced to cast from long to int since the skip method in entity framework does not accept a long argument
            int commitsToSkip = (initialCommitExclusive < 0) ? 0 : (int)initialCommitExclusive;

            var revisions = await DbContext.StoredRevisions
                .OrderBy(t => t.CommitId)
                .Skip(commitsToSkip)
                .Take(numberOfResults)
                .ToListAsync();

            return revisions;
        }

        #endregion

        #region Properties

        public IEventStoreDbContext DbContext
        {
            get
            {
                return this.dbContext;
            }
            set
            {
                //TODO: Add validation code
                this.dbContext = value;
            }
        }

        private ILogger Logger
        {
            get
            {
                return this.logger;
            }
            set
            {
                //TODO: Add validation code
                this.logger = value;
            }
        }

        #endregion
    }
}
