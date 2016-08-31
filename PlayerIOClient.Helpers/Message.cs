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
            var _dict = new Dictionary<string, object>();
            var _properties = new Dictionary<uint, Dictionary<string, object>>();

            for (uint i = 0; i < message.Count; i++)
                _properties.Add(i, new Dictionary<string, object>() { { message[i].GetType().FullName, message[i] } });

            _dict.Add("type", message.Type);
            _dict.Add("properties", _properties);

            return _dict;
        }

        public static string Serialize(this Message message)
        {
            return JsonConvert.SerializeObject(message.ToDictionary());
        }

        public static Message Deserialize(this Message message, string input)
        {
            var _dict = JObject.Parse(input);
            var _properties = _dict["properties"].ToObject<Dictionary<uint, Dictionary<string, object>>>();
            var _message = Message.Create((string)_dict["type"]);

            foreach (var dictionary in _properties.Values) {
                foreach (var property in dictionary) {
                    var type = Type.GetType(property.Key, false);
                    var value = property.Value;

                    _message.Add((type == null) ? value :
                        (type.FullName == "System.Byte[]") ? Convert.ChangeType(Convert.FromBase64String(value as string), type) :
                        Convert.ChangeType(value, type));
                }
            }

            return _message;
        }
    }
}
