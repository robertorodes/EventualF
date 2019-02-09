using Eventual.Common.Logging;
using Eventual.EventStore.Core;
using Eventual.EventStore.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eventual.EventStore.Services
{
    public class TypedEventStore : ITypedEventStore
    {
        #region Attributes

        IEventStore eventStore;
        IEventsSerializer eventsSerializer;
        IMetadataSerializer metadataSerializer;
        ILogger logger;

        #endregion

        #region Constructors

        public TypedEventStore(IEventStore eventStore, IEventsSerializer eventsSerializer, IMetadataSerializer metadataSerializer, ILogger logger)
        {
            this.EventStore = eventStore;
            this.EventsSerializer = eventsSerializer;
            this.MetadataSerializer = metadataSerializer;
            this.Logger = logger;
        }

        #endregion

        #region Methods

        public async Task CommitChangesAsync(TypedEventStreamCommit eventStreamForCommit)
        {
            byte[] revisionData = this.EventsSerializer.Serialize(eventStreamForCommit.Changes);
            byte[] metadata = this.MetadataSerializer.Serialize(eventStreamForCommit.Metadata);

            EventStreamCommit commit = new EventStreamCommit(eventStreamForCommit.AggregateId,
                eventStreamForCommit.AggregateType, eventStreamForCommit.WorkingCopyVersion,
                eventStreamForCommit.CorrelationId, eventStreamForCommit.CausationId,
                this.MetadataSerializer.ContentType, this.MetadataSerializer.ContentEncoding, metadata,
                this.EventsSerializer.ContentType, this.EventsSerializer.ContentEncoding, revisionData);

            await this.EventStore.CommitChangesAsync(commit);
        }

        public async Task<IEnumerable<EsRevision>> GetEventStreamAsync(Guid aggregateId)
        {
            var rawEventStream = await this.EventStore.GetEventStreamAsync(aggregateId);

            //TODO: Take into account ContentEncoding and ContentType coming in the raw event stream for deserialization of changes and metadata

            IList<EsRevision> eventStream = new List<EsRevision>();
            foreach (var r in rawEventStream)
            {
                eventStream.Add(new EsRevision(r.AggregateId, r.RevisionId, r.CommitId, r.AggregateType,
                    r.OccurrenceDate, r.CorrelationId, r.CausationId, this.MetadataSerializer.Deserialize(r.Metadata),
                    this.EventsSerializer.Deserialize(r.Changes)));
            }

            return eventStream;
        }

        public async Task<IEnumerable<EsRevision>> GetEventStreamFromAsync(Guid aggregateId, int initialRevision, int numberOfResults)
        {
            var rawEventStream = await this.EventStore.GetEventStreamFromAsync(aggregateId, initialRevision, numberOfResults);

            //TODO: Take into account ContentEncoding and ContentType coming in the raw event stream for deserialization of changes and metadata

            IList<EsRevision> eventStream = new List<EsRevision>();
            foreach (var r in rawEventStream)
            {
                eventStream.Add(new EsRevision(r.AggregateId, r.RevisionId, r.CommitId, r.AggregateType,
                    r.OccurrenceDate, r.CorrelationId, r.CausationId, this.MetadataSerializer.Deserialize(r.Metadata),
                    this.EventsSerializer.Deserialize(r.Changes)));
            }

            return eventStream;
        }

        public async Task<IEnumerable<EsRevision>> GetAllEventStreamsFromAsync(int initialCommit, int numberOfResults)
        {
            var rawEventStream = await this.EventStore.GetAllEventStreamsFromAsync(initialCommit, numberOfResults);

            //TODO: Take into account ContentEncoding and ContentType coming in the raw event stream for deserialization of changes and metadata

            IList<EsRevision> eventStream = new List<EsRevision>();
            foreach (var r in rawEventStream)
            {
                eventStream.Add(new EsRevision(r.AggregateId, r.RevisionId, r.CommitId, r.AggregateType,
                    r.OccurrenceDate, r.CorrelationId, r.CausationId, this.MetadataSerializer.Deserialize(r.Metadata),
                    this.EventsSerializer.Deserialize(r.Changes)));
            }

            return eventStream;
        }

        #endregion

        #region Properties

        private IEventStore EventStore
        {
            get
            {
                return this.eventStore;
            }
            set
            {
                //TODO: Add validation code
                this.eventStore = value;
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

        private IEventsSerializer EventsSerializer
        {
            get
            {
                return this.eventsSerializer;
            }
            set
            {
                //TODO: Add validation code
                this.eventsSerializer = value;
            }
        }

        private IMetadataSerializer MetadataSerializer
        {
            get
            {
                return this.metadataSerializer;
            }
            set
            {
                //TODO: Add validation code
                this.metadataSerializer = value;
            }
        }

        #endregion
    }
}
