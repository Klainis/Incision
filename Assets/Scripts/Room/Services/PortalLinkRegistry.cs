using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Incision.Room.Runtime;
using UnityEngine;

namespace Incision.Room.Services
{
    public class PortalLinkRegistry
    {
        private readonly Dictionary<string, List<PortalLink>> _byRoom = new();

        public IReadOnlyList<PortalLink> GetLinks(string roomId) =>
            _byRoom.TryGetValue(roomId, out var l) ? l : System.Array.Empty<PortalLink>();

        public void Register(PortalLink link)
        {
            if (!_byRoom.TryGetValue(link.SourceRoomId, out var list))
                _byRoom[link.SourceRoomId] = list = new List<PortalLink>();
            list.Add(link);
        }

        public void UnregisterRoom(string roomId) => _byRoom.Remove(roomId);

        public IEnumerable<PortalLink> All => _byRoom.Values.SelectMany(l => l);
    }
}
