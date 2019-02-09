using Eventual.EventStore.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eventual.EventStore.Serialization.Json
{
    public class JsonEventsSerializer : IEventsSerializer
    {
        #region Constants

        private const string ContentTypeConstant = "application/json";

        #endregion

        #region Attributes

        readonly static Lazy<JsonEventsSerializer> instance = new Lazy<JsonEventsSerializer>(() => new JsonEventsSerializer(), true);
        readonly bool IsPretty;

        #endregion

        #region Constructors

        public JsonEventsSerializer(bool isPretty = false)
        {
            this.IsPretty = isPretty;
        }

        #endregion

        #region Methods

        public byte[] Serialize(IEnumerable<Event> events)
        {
            IEnumerable<SerializedEvent> serializedEvents = events.Select(t => new SerializedEvent
            {
                Type = t.GetType().AssemblyQualifiedName,
                SchemaVersion = t.SchemaVersion,
                Event = t
            });

            string json = JsonConvert.SerializeObject(serializedEvents, this.IsPretty ? Formatting.Indented : Formatting.None);

            return this.GetBytes(json);
        }

        public IEnumerable<Event> Deserialize(byte[] serializedEvents)
        {
            IList<Event> deserializedEvents = new List<Event>();

            string stringSerializedEvents = this.GetString(serializedEvents);
            JArray events = JArray.Parse(stringSerializedEvents);
            foreach (var @event in events)
            {
                Type t = Type.GetType((string)@event["Type"]);
                int schemaVersion = (int)@event["SchemaVersion"];
                deserializedEvents.Add((Event)@event["Event"].ToObject(t));
            }

            return deserializedEvents;
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

        public static JsonEventsSerializer Instance
        {
            get
            {
                return instance.Value;
            }
        }

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

        private class SerializedEvent
        {
            public string Type { get; set; }

            public int SchemaVersion { get; set; }

            public Event Event { get; set; }
        }
    }
}
