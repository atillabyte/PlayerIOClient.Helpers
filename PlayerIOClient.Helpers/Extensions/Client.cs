using PlayerIOClient.Helpers.Helpers;

namespace PlayerIOClient.Helpers
{
    public static class ClientExtensions
    {
        public static Client SetToken(this Client client, string token) { PropertyHelper.SetPrivatePropertyValue(Reflection.GetFieldValue(client, "channel"), "Token", token); return client; }
        public static string GetToken(this Client client) => PropertyHelper.GetPrivatePropertyValue<string>(Reflection.GetFieldValue(client, "channel"), "Token");
    }
}