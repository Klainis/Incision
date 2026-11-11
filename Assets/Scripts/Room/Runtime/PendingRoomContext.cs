namespace Incision.Room.Runtime
{
    public static class PendingRoomNode
    {
        private static RoomNode _value;

        public static void Set(RoomNode node) => _value = node;

        public static bool TryConsume(out RoomNode node)
        {
            node = _value;
            _value = null;
            return node != null;
        }
    }
}