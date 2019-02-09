using Eventual.EventStore.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Eventual.EventStore.Readers
{
    public class EventStream
    {
        #region Attributes

        private IList<Revision> revisions;

        #endregion

        #region Constructors

        public EventStream(IList<Revision> revisions)
        {
            this.Revisions = revisions;
        }

        #endregion

        #region Properties

        public IList<Revision> Revisions
        {
            get
            {
                return new ReadOnlyCollection<Revision>(this.revisions);
            }
            private set
            {
                //TODO: Insert validation code here
                this.revisions = value;
            }
        }

        #endregion
    }
}
