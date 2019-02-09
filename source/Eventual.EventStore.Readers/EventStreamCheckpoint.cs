using System;
using System.Collections.Generic;
using System.Text;

namespace Eventual.EventStore.Readers
{
    public class EventStreamCheckpoint
    {
        #region Attributes

        private string streamId;
        private int revisionId;
        private int eventId;

        #endregion

        #region Constructors

        private EventStreamCheckpoint() { }

        private EventStreamCheckpoint(string streamId, int revisionId, int eventId)
        {
            this.StreamId = streamId;
            this.RevisionId = revisionId;
            this.EventId = eventId;
        }

        #endregion 

        #region Factory methods

        public static EventStreamCheckpoint CreateStreamCheckpoint(string streamId, int revisionId, int eventId)
        {
            return new EventStreamCheckpoint(streamId, revisionId, eventId);
        }

        public static EventStreamCheckpoint Empty
        {
            get
            {
                return CreateStreamCheckpoint(null, 0, 0);
            }
        }

        #endregion

        #region Properties

        public string StreamId
        {
            get
            {
                return streamId;
            }
            private set
            {
                //TODO: Insert validation code here
                streamId = value;
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
                //TODO: Insert validation code here
                this.revisionId = value;
            }
        }

        public int EventId
        {
            get
            {
                return eventId;
            }
            private set
            {
                //TODO: Insert validation code here
                eventId = value;
            }
        }

        #endregion
    }
}
