using System;
using System.Collections.Generic;
using System.Text;

namespace Eventual.EventStore.Services
{
    public class Event
    {
        #region Constructors

        public Event(int schemaVersion)
        {
            this.SchemaVersion = schemaVersion;
            this.OccurrenceDate = DateTime.UtcNow;
        }

        #endregion

        #region Properties

        public int SchemaVersion { get; protected set; }

        public DateTime OccurrenceDate { get; protected set; }

        #endregion
    }
}
