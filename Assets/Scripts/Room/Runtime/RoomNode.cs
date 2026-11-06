using System;
using System.Collections.Generic;
using Incision.Room.Templates;
using UnityEngine;

namespace Incision.Room.Runtime
{
    public class RoomNode
    {
        public readonly string Id;
        public readonly RoomTemplate Template;
        public Vector2Int GridPosition;
        public readonly List<string> Connections;

        public RoomNode(RoomTemplate template, Vector2Int gridPosition)
        {
            Id = Guid.NewGuid().ToString();
            Template = template;
            GridPosition = gridPosition;
            Connections = new List<string>();
        }

        public bool CanAcceptPortal => Connections.Count < Template.MaxPortals;
    }
}
