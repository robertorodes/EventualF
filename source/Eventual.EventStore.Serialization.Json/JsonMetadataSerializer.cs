using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Eventual.EventStore.Serialization.Json
{
    public class JsonMetadataSerializer : IMetadataSerializer
    {
        #region Constants

        private const string ContentTypeConstant = "application/json";

        #endregion

        #region Attributes

        readonly static Lazy<JsonEventsSerializer> instance = new Lazy<JsonEventsSerializer>(() => new JsonEventsSerializer(), true);
        readonly bool IsPretty;

        #endregion

        #region Constructors

        public JsonMetadataSerializer(bool isPretty = false)
        {
            this.IsPretty = false;
        }

        #endregion

        #region Methods

        public byte[] Serialize(Dictionary<string, string> metadata)
        {
            string json = JsonConvert.SerializeObject(metadata, this.IsPretty ? Formatting.Indented : Formatting.None);

            return this.GetBytes(json);
        }

        public Dictionary<string, string> Deserialize(byte[] encodedMetadata)
        {
            string encodedStringMetadata = this.GetString(encodedMetadata);

            return JsonConvert.DeserializeObject<Dictionary<string, string>>(encodedStringMetadata);
        }

        private byte[] GetBytes(string str)
        {
            return System.Text.Encoding.UTF8.GetBytes(str);
        }

        private string GetString(byte[] bytes)
        {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        #endregion

        #region Properties

        public string ContentType
        {
            get
            {
                return ContentTypeConstant;
            }
        }

        public string ContentEncoding
        {
            get
            {
                return System.Text.Encoding.UTF8.HeaderName;
            }
        }

        #endregion
    }
}
