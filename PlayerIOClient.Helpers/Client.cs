using PlayerIOClient.Helpers.Helpers;

namespace PlayerIOClient.Helpers
{
    public static class ClientExtensions
    {
        public static void SetToken(this Client client, string token) => PropertyHelper.SetPrivatePropertyValue(Reflection.GetFieldValue(client, "channel"), "Token", token);
        public static string GetToken(this Client client) => PropertyHelper.GetPrivatePropertyValue<string>(Reflection.GetFieldValue(client, "channel"), "Token");
    }
}