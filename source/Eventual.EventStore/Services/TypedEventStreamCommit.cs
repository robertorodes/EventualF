using System;
using System.Collections.Generic;
using System.Text;

namespace Eventual.EventStore.Services
{
    public class TypedEventStreamCommit
    {
        #region Attributes

        private Guid aggregateId;
        private Type aggregateType;
        private int workingCopyVersion;
        private string correlationId;
        private string causationId;
        private Dictionary<string, string> metadata;
        private IEnumerable<Event> changes;

        #endregion

        #region Constructors

        public TypedEventStreamCommit(Guid aggregateId, Type aggregateType, int workingCopyVersion, string correlationId, string causationId,
            Dictionary<string, string> metadata, IEnumerable<Event> changes)
        {
            this.AggregateId = aggregateId;
            this.AggregateType = aggregateType;
            this.WorkingCopyVersion = workingCopyVersion;
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

        public Type AggregateType
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

        //TODO: Add here new property "AggregateState" for supporting snapshots (but maybe it should be treated from the upper layer -repository-, since it must be parsed and deserialized to the current aggregate specific schema).

        public int WorkingCopyVersion
        {
            get
            {
                return this.workingCopyVersion;
            }
            private set
            {
                //TODO: Add validation code
                this.workingCopyVersion = value;
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
