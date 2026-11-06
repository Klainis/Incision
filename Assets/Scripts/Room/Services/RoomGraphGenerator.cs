using System.Collections.Generic;
using System.Linq;
using Incision.Extensions;
using Incision.Interfaces;
using Incision.Room.Runtime;
using Incision.Room.Services.Interfaces;
using Incision.Room.Templates;
using Incision.Structs.Events;
using UnityEngine;
using VContainer;

namespace Incision.Room.Services
{
    public class RoomGraphGenerator : IRoomGraphGenerator, ITaggable
    {
        [Inject] private IEventBus _eventBus;
        private RoomCatalog _catalog;
        private int _maxSpawnAttempts;

        private RoomGraphGenerator(RoomGraphTemplate template)
        {
            _catalog = template.Catalog;
            _maxSpawnAttempts = template.MaxSpawnAttempts;
        }
        
        private static readonly Vector2Int[] Dirs =
        {
            new(0, 1), new(0, -1), new(1, 0), new(-1, 0)
        };

        public RoomGraph Generate(int roomCount)
        {
            var graph = new RoomGraph();

            var start = new RoomNode(_catalog.StartRoom, Vector2Int.zero);
            graph.Nodes[start.Id] = start;
            graph.StartNodeId = start.Id;

            var frontier = new List<RoomNode> { start };
            var guard = 0;

            while (graph.Nodes.Count - 1 < roomCount && guard++ < _maxSpawnAttempts)
            {
                var sources = frontier.Where(n => n.CanAcceptPortal).ToList();
                if (sources.Count == 0) break;
                
                var from = sources[Random.Range(0, sources.Count)];

                var dirs = Dirs.OrderBy(_ => Random.value).ToArray();
                RoomNode created = null;
                
                foreach (var d in dirs)
                {
                    var pos = from.GridPosition + d;
                    _eventBus.Publish(new Log(LOG_TAG,$"Generating room at {pos}"));
                    if (graph.Nodes.Values.Any(n => n.GridPosition == pos))
                    {
                        _eventBus.Publish(new LogWarning(LOG_TAG,$"Cannot generate room at {pos}"));
                        continue;
                    }

                    var tpl = _catalog.GetRandomRoom();
                    if (!tpl)
                    {
                        _eventBus.Publish(new LogWarning(LOG_TAG,$"Cannot get template for room"));
                        continue;
                    }
                    
                    var node = new RoomNode(tpl, pos);
                    
                    from.Connections.Add(node.Id);
                    
                    graph.Nodes[node.Id] = node;
                    frontier.Add(node);
                    created = node;
                    _eventBus.Publish(new Log(LOG_TAG,$"Room generated {node.Id} to {tpl.SceneName}"));
                    break;
                }
                var end = new RoomNode(_catalog.EndRoom, Vector2Int.zero);
                graph.Nodes[end.Id] = end;
                if (created == null) break;
            }
            
            _eventBus.Publish(new Log(LOG_TAG,$"{JsonUtility.ToJson(graph)}"));
            return graph;
        }

        public string LOG_TAG { get; } = "[GraphGenerator]".AsTag(Color.grey);
    }
}
