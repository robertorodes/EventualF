using Eventual.EventStore.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eventual.EventStore.Readers
{
    public class InProcessEventStreamReader : IEventStreamReader
    {
        #region Attributes

        private const int MaxResultsPerRequest = 50;
        private IEventStore eventStore;

        #endregion

        #region Constructors

        public InProcessEventStreamReader(IEventStore eventStore)
        {
            this.EventStore = eventStore;
        }

        #endregion

        #region Methods

        public async Task<EventStream> GetEventStreamFromAsync(string streamId, int initialRevisionId, int numberOfResults)
        {
            //TODO: Implement the rest of required validations here
            if (numberOfResults > MaxResultsPerRequest)
            {
                throw new ArgumentException(string.Format("The number of results requested cannot be higher than {0}.", numberOfResults), "numberOfResults");
            }

            var revisions = await this.EventStore.GetEventStreamFromAsync(new Guid(streamId), initialRevisionId, numberOfResults);

            return new EventStream(revisions);
        }

        public async Task<EventStream> GetAllEventStreamsFromAsync(long initialCommitId, int numberOfResults)
        {
            //TODO: Implement the rest of required validations here
            if (numberOfResults > MaxResultsPerRequest)
            {
                throw new ArgumentException(string.Format("The number of results requested cannot be higher than {0}.", numberOfResults), "numberOfResults");
            }

            var revisions = await this.EventStore.GetAllEventStreamsFromAsync(initialCommitId, numberOfResults);

            return new EventStream(revisions);
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
                //TODO: Insert validation code here
                this.eventStore = value;
            }
        }

        #endregion
    }
}
