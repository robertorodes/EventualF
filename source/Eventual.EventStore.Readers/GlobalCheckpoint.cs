using System;
using System.Collections.Generic;
using System.Text;

namespace Eventual.EventStore.Readers
{
    public class GlobalCheckpoint
    {
        #region Attributes

        private long commitId;

        #endregion

        #region Constructors

        private GlobalCheckpoint() { }

        private GlobalCheckpoint(long commitId)
        {
            this.CommitId = commitId;
        }

        #endregion

        #region Factory methods

        public static GlobalCheckpoint CreateFromStart()
        {
            return Create(0);
        }

        public static GlobalCheckpoint Create(long commitId)
        {
            return new GlobalCheckpoint(commitId);
        }

        public static GlobalCheckpoint Empty
        {
            get
            {
                return Create(0);
            }
        }

        #endregion

        #region Properties

        public long CommitId
        {
            get
            {
                return this.commitId;
            }
            private set
            {
                //TODO: Insert validation code here
                this.commitId = value;
            }
        }

        #endregion
    }
}
