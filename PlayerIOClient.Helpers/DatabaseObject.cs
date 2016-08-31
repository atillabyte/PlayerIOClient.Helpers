using System.Collections.Generic;

namespace PlayerIOClient.Helpers
{
    public static class DatabaseObjectExtensions
    {
        private static Dictionary<object, object> ToDictionary(this DatabaseObject dbo, object input)
        {
            var _dict = new Dictionary<object, object>();
            var _spec = new List<string>() { "DatabaseObject", "identifier916", "DatabaseArray", "identifier917" };

            switch (input.GetType().Name) {
                case "DatabaseObject":
                case "identifier916":
                    foreach (var o in ((DatabaseObject)input)) {
                        _dict.Add(o.Key, _spec.Contains(o.Value.GetType().Name) ? ToDictionary(null, o.Value) : o.Value);
                    }
                    break;
                case "DatabaseArray":
                case "identifier917":
                    foreach (var o in ((DatabaseArray)input).IndexesAndValues)
                        _dict.Add(o.Key, _spec.Contains(o.Value.GetType().Name) ? ToDictionary(null, o.Value) : o.Value);
                    break;
            }

            return _dict;
        }

        public static Dictionary<object, object> ToDictionary(this DatabaseObject dbo) => ToDictionary(dbo, dbo);
        public static string SerializeToJson(this DatabaseObject dbo) => Newtonsoft.Json.JsonConvert.SerializeObject(ToDictionary(dbo, dbo));
    }
}
