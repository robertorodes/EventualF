using System;
using System.Collections.Generic;
using System.Text;

namespace Eventual.EventStore.Serialization
{
    public interface IMetadataSerializer
    {
        byte[] Serialize(Dictionary<string, string> metadata);

        Dictionary<string, string> Deserialize(byte[] encodedMetadata);

        string ContentType { get; }

        string ContentEncoding { get; }
    }
}
