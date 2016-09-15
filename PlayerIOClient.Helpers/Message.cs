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
            var properties = new Dictionary<uint, Dictionary<string, object>>();

            for (uint i = 0; i < message.Count; i++)
                properties.Add(i, new Dictionary<string, object>() { { message[i].GetType().FullName, message[i] } });

            dict.Add("type", message.Type);
            dict.Add("properties", properties);

            return dict;
        }

        public static string Serialize(this Message message) => JsonConvert.SerializeObject(message.ToDictionary());

        public static Message Deserialize(this Message message, string input)
        {
            var dict = JObject.Parse(input);
            var properties = dict["properties"].ToObject<Dictionary<uint, Dictionary<string, object>>>();
            var output = Message.Create((string)dict["type"]);

            foreach (var dictionary in properties.Values) {
                foreach (var property in dictionary) {
                    var type = Type.GetType(property.Key, false);
                    var value = property.Value;

                    output.Add(type == null ? value :
                        type.FullName == "System.Byte[]" ? Convert.ChangeType(Convert.FromBase64String(value as string), type)
                        : Convert.ChangeType(value, type));
                }
            }

            return output;
        }
    }
}
