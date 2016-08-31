using System;
using System.Reflection;

namespace PlayerIOClient.Helpers.Helpers
{
    public static class Reflection
    {
        public static object GetInstanceField(Type type, object instance, string fieldName) =>
            type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(instance);
    }
}
