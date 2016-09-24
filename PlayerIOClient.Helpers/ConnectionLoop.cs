using System.Linq;

namespace PlayerIOClient.Helpers
{
    public static class ConnectionLoop
    {
        private static CircularList<Connection> _connections = new CircularList<Connection>() { Loop = true };
        public static Connection[] Collection => _connections.ToArray();

        public static int Connected => _connections.Count(x => x.Connected);
        public static int Count => _connections.Count;
        
        public static void Setup(params Connection[] connections)
        {
            _connections.Clear();
            _connections.AddRange(connections);
        }

        public static void Send(string type, params object[] input) => Send(Message.Create(type, input));

        public static void Send(Message message)
        {
            var next = _connections.Next;
            if (next.Connected)
                next.Send(message);
            else
                Send(message);
        }

        public static void Empty() => _connections.Clear();
    }
}