using System;
using System.Collections.Generic;
using System.Text;

namespace Eventual.EventStore.Services
{
    public class EsRevision
    {
        #region Attributes

        private Guid aggregateId;
        private int revisionId;
        private long commitId;
        private string aggregateType;
        private DateTime occurrenceDate;
        private string correlationId;
        private string causationId;
        private Dictionary<string, string> metadata;
        private IEnumerable<Event> changes;

        #endregion

        #region Constructors

        public EsRevision()
        { }

        public EsRevision(Guid aggregateId, int revisionId, long commitId, string aggregateType, DateTime occurrenceDate, string correlationId, string causationId,
            Dictionary<string, string> metadata, IEnumerable<Event> changes)
        {
            this.AggregateId = aggregateId;
            this.RevisionId = revisionId;
            this.CommitId = commitId;
            this.AggregateType = aggregateType;
            this.OccurrenceDate = occurrenceDate;
            this.CorrelationId = correlationId;
            this.CausationId = causationId;
            this.Metadata = metadata;
            this.Changes = changes;
        }

        #endregion

        #region Properties            

        public Guid AggregateId
        {
            get
            {
                return this.aggregateId;
            }
            private set
            {
                //TODO: Add validation code
                this.aggregateId = value;
            }
        }

        public int RevisionId
        {
            get
            {
                return this.revisionId;
            }
            private set
            {
                //TODO: Add validation code
                this.revisionId = value;
            }
        }

        public long CommitId
        {
            get
            {
                return this.commitId;
            }
            private set
            {
                //TODO: Add validation code
                this.commitId = value;
            }
        }

        public string AggregateType
        {
            get
            {
                return this.aggregateType;
            }
            private set
            {
                //TODO: Add validation code
                this.aggregateType = value;
            }
        }

        public DateTime OccurrenceDate
        {
            get
            {
                return this.occurrenceDate;
            }
            private set
            {
                //TODO: Add validation code
                this.occurrenceDate = value;
            }
        }


        public string CorrelationId
        {
            get
            {
                return this.correlationId;
            }
            private set
            {
                //TODO: Add validation code
                this.correlationId = value;
            }
        }

        public string CausationId
        {
            get
            {
                return this.causationId;
            }
            private set
            {
                //TODO: Add validation code
                this.causationId = value;
            }
        }

        public Dictionary<string, string> Metadata
        {
            get
            {
                return this.metadata;
            }
            private set
            {
                //TODO: Add validation code
                this.metadata = value;
            }
        }

        public IEnumerable<Event> Changes
        {
            get
            {
                return this.changes;
            }
            private set
            {
                //TODO: Add validation code
                this.changes = value;
            }
        }

        #endregion
    }
}
