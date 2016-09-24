using System.Collections.Generic;

namespace PlayerIOClient.Helpers
{
    public static class DatabaseObjectExtensions
    {
        public static string Version(this DatabaseObject input) => (string)Helpers.Reflection.GetInstanceField(typeof(DatabaseObject), input, "Version");

        private static Dictionary<object, object> ToDictionary(this DatabaseObject dbo, object input)
        {
            var dict = new Dictionary<object, object>();
            var spec = new List<string>() { "DatabaseObject", "identifier916", "DatabaseArray", "identifier917" };

            switch (input.GetType().Name) {
                case "DatabaseObject":
                case "identifier916":
                    foreach (var o in ((DatabaseObject)input))
                        dict.Add(o.Key, spec.Contains(o.Value.GetType().Name) ? ToDictionary(null, o.Value) : o.Value);
                    break;
                case "DatabaseArray":
                case "identifier917":
                    foreach (var o in ((DatabaseArray)input).IndexesAndValues)
                        dict.Add(o.Key, spec.Contains(o.Value.GetType().Name) ? ToDictionary(null, o.Value) : o.Value);
                    break;
            }

            return dict;
        }

        public static Dictionary<object, object> ToDictionary(this DatabaseObject dbo) => ToDictionary(dbo, dbo);
    }
}