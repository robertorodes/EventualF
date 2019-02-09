using System;
using System.Collections.Generic;
using System.Text;

namespace Eventual.EventStore.Core
{
    public class EventStreamCommit
    {
        #region Attributes

        private Guid aggregateId;
        private Type aggregateType;
        private int workingCopyVersion;
        private string correlationId;
        private string causationId;
        private string metadataContentType;
        private string metadataContentEncoding;
        private byte[] metadata;
        private string changesContentType;
        private string changesContentEncoding;
        private byte[] changes;

        #endregion

        #region Constructors

        public EventStreamCommit(Guid aggregateId, Type aggregateType, int workingCopyVersion, string correlationId, string causationId,
            string metadataContentType, string metadataContentEncoding, byte[] metadata,
            string changesContentType, string changesContentEncoding, byte[] changes)
        {
            this.AggregateId = aggregateId;
            this.AggregateType = aggregateType;
            this.WorkingCopyVersion = workingCopyVersion;
            this.CorrelationId = correlationId;
            this.CausationId = causationId;
            this.MetadataContentType = metadataContentType;
            this.MetadataContentEncoding = metadataContentEncoding;
            this.Metadata = metadata;
            this.ChangesContentType = changesContentType;
            this.ChangesContentEncoding = changesContentEncoding;
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

        public string MetadataContentType
        {
            get
            {
                return this.metadataContentType;
            }
            private set
            {
                //TODO: Add validation code
                this.metadataContentType = value;
            }
        }

        public string MetadataContentEncoding
        {
            get
            {
                return this.metadataContentEncoding;
            }
            private set
            {
                //TODO: Add validation code
                this.metadataContentEncoding = value;
            }
        }

        public byte[] Metadata
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

        public string ChangesContentType
        {
            get
            {
                return this.changesContentType;
            }
            private set
            {
                //TODO: Add validation code
                this.changesContentType = value;
            }
        }

        public string ChangesContentEncoding
        {
            get
            {
                return this.changesContentEncoding;
            }
            private set
            {
                //TODO: Add validation code
                this.changesContentEncoding = value;
            }
        }

        public byte[] Changes
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
