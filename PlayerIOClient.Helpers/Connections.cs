namespace PlayerIOClient.Helpers
{
    public class Connections
    {
        private static CircularList<Connection> _connections = new CircularList<Connection>() { Loop = true };
        private static int _count = 0;

        public static int Count => _count;
        public static Connection[] Collection => _connections.ToArray();

        public static void Setup(params Connection[] connections)
        {
            _connections.AddRange(connections);
            _count = connections.Length;
        }

        public static void Empty() => _connections.Clear();

        public static void Send(Message message)
        {
            var next = _connections.Next;
            if (next.Connected)
                next.Send(message);
            else
                Send(message);
        }

        public static void Send(string type, params object[] input) => Send(Message.Create(type, input));
    }
}