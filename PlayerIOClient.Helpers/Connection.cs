using System;
using System.Net;
using System.Net.Sockets;
using PlayerIOClient.Helpers.Helpers;

namespace PlayerIOClient.Helpers
{
    public static class ConnectionExtensions
    {
        public class Bandwidth
        {
            public int SentBytes { get; set; }
            public int ReceivedBytes { get; set; }
            public DateTime WindowStart { get; set; }
        }

        public static Bandwidth GetBandwidth(this Connection connection) => new Bandwidth() {
            SentBytes = (int)Reflection.GetFieldValue(connection, "con").GetPrivatePropertyValue<object>("identifier1000").GetFieldValue("sentBytes"),
            ReceivedBytes = (int)Reflection.GetFieldValue(connection, "con").GetPrivatePropertyValue<object>("identifier1000").GetFieldValue("receivedBytes"),
            WindowStart = (DateTime)Reflection.GetFieldValue(connection, "con").GetPrivatePropertyValue<object>("identifier1000").GetFieldValue("windowStart"),
        };

        public static IPEndPoint GetEndPoint(this Connection connection) =>
            (IPEndPoint)Reflection.GetFieldValue(connection, "con").GetPrivatePropertyValue<EndPoint>("identifier998");

        public static Socket GetSocket(this Connection connection) =>
            (Socket)Reflection.GetFieldValue(connection, "con").GetPrivateFieldValue<Socket>("socket");
    }
}
