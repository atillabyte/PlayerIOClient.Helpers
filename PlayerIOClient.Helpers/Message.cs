using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace PlayerIOClient.Helpers
{
    public static class MessageExtensions
    {
        public static Dictionary<string, object> ToDictionary(this Message message)
        {
            var dict = new Dictionary<string, object>();
            var properties = new List<object>();

            for (uint i = 0; i < message.Count; i++)
                properties.Add(message[i] is byte[] ? new[] { Convert.ToBase64String(message[i] as byte[]) } : message[i]);

            dict.Add("type", message.Type);
            dict.Add("properties", properties);

            return dict;
        }
    }

    public static class MessageConverter
    {
        public static string Serialize(this Message message) => JsonConvert.SerializeObject(message.ToDictionary());

        public static Message Deserialize(string input)
        {
            var dict = JObject.Parse(input);
            var properties = dict["properties"].ToObject<List<object>>();
            var output = Message.Create((string)dict["type"]);

            foreach (var value in properties) {
                if (value is JArray)
                    output.Add(Convert.FromBase64String(((JArray)value)[0].Value<string>()));
                else
                    output.Add(value);
            }

            return output;
        }
    }
}