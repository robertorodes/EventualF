using System;
using System.Collections.Generic;
using System.Text;

namespace Eventual.EventStore.Core
{
    public class Revision
    {
        #region Properties

        public long CommitId { get; set; }

        public DateTime OccurrenceDate { get; set; }

        public string AggregateType { get; set; }

        public Guid AggregateId { get; set; }

        public int RevisionId { get; set; }

        public string CorrelationId { get; set; }

        public string CausationId { get; set; }

        public string MetadataContentType { get; set; }

        public string MetadataContentEncoding { get; set; }

        public byte[] Metadata { get; set; }

        public string ChangesContentType { get; set; }

        public string ChangesContentEncoding { get; set; }

        public byte[] Changes { get; set; }

        #endregion

        #region Public methods

        public override string ToString()
        {
            return string.Format("[AggregateId: {0}, RevisionId: {1}, CommitId: {2}, AggregateType: {3}, OccurrenceDate: {4}, CorrelationId: {5}, CausationId: {6}]",
                AggregateId, RevisionId, CommitId, AggregateType, OccurrenceDate, CorrelationId, CausationId);
        }

        #endregion
    }
}
