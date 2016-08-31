namespace PlayerIOClient.Helpers
{
    public static class ConnectionExtensions
    {
        public static void TrySend(this Connection connection, Message message) { if (connection.Connected) connection.Send(message); }
        public static void TrySend(this Connection connection, string type, params object[] input) { TrySend(connection, Message.Create(type, input)); }
    }
}
