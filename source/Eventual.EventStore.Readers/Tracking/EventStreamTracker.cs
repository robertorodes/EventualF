using System;
using System.Collections.Generic;
using System.Text;

namespace Eventual.EventStore.Readers.Tracking
{
    public class EventStreamTracker
    {
        #region Attributes

        private string trackerId;
        private CheckpointType checkpointType;
        private GlobalCheckpoint globalCheckpoint;
        private EventStreamCheckpoint streamCheckpoint;

        #endregion

        #region Constructors

        private EventStreamTracker() { }

        public EventStreamTracker(string trackerId, EventStreamCheckpoint checkpoint)
        {
            this.TrackerId = trackerId;
            this.StreamCheckpoint = checkpoint;
            this.Type = Readers.CheckpointType.StreamCheckpoint;
            this.GlobalCheckpoint = Readers.GlobalCheckpoint.Empty;
        }

        public EventStreamTracker(string trackerId, GlobalCheckpoint checkpoint)
        {
            this.TrackerId = trackerId;
            this.GlobalCheckpoint = checkpoint;
            this.Type = Readers.CheckpointType.GlobalCheckpoint;
            this.StreamCheckpoint = Readers.EventStreamCheckpoint.Empty;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        public string TrackerId
        {
            get
            {
                return this.trackerId;
            }
            private set
            {
                //TODO: Insert validation code here
                this.trackerId = value;
            }
        }

        public CheckpointType Type
        {
            get
            {
                return this.checkpointType;
            }
            private set
            {
                //TODO: Insert validation code here
                this.checkpointType = value;
            }
        }

        public GlobalCheckpoint GlobalCheckpoint
        {
            get
            {
                return this.globalCheckpoint;
            }
            private set
            {
                //TODO: Insert validation code here
                this.globalCheckpoint = value;
            }
        }

        public EventStreamCheckpoint StreamCheckpoint
        {
            get
            {
                return this.streamCheckpoint;
            }
            private set
            {
                //TODO: Insert validation code here
                this.streamCheckpoint = value;
            }
        }

        public byte[] RowVersion { get; private set; }

        #endregion

        internal void UpdateCheckpoint(Readers.GlobalCheckpoint checkpoint)
        {
            this.GlobalCheckpoint = checkpoint;
        }
    }
}
