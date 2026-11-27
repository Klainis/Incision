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
        public string LOG_TAG { get; } = "[GraphGenerator]".AsTag(Color.grey);

        private RoomCatalog _catalog;
        private RoomGraphTemplate _template;

        private int _maxSpawnAttempts;

        [Inject] private IEventBus _eventBus;

        private RoomGraphGenerator(RoomGraphTemplate template)
        {
            _catalog = template.Catalog;
            _maxSpawnAttempts = template.MaxSpawnAttempts;
        }
        
        private static readonly Vector2Int[] Dirs =
        {
            new(0, 1), new(0, -1), new(1, 0), new(-1, 0)
        };

        //public RoomGraph Generate(int roomCount)
        //{
        //    var graph = new RoomGraph();

        //    var start = new RoomNode(_catalog.StartRoom, Vector2Int.zero);
        //    graph.Nodes[start.Id] = start;
        //    graph.StartNodeId = start.Id;

        //    var frontier = new List<RoomNode> { start };
        //    var guard = 0;

        //    while (graph.Nodes.Count - 1 < roomCount && guard++ < _maxSpawnAttempts)
        //    {
        //        var sources = frontier.Where(n => n.CanAcceptPortal).ToList();
        //        if (sources.Count == 0) break;

        //        var from = sources[Random.Range(0, sources.Count)];

        //        var dirs = Dirs.OrderBy(_ => Random.value).ToArray();
        //        RoomNode created = null;

        //        foreach (var d in dirs)
        //        {
        //            var pos = from.GridPosition + d;
        //            _eventBus.Publish(new Log(LOG_TAG,$"Generating room at {pos}"));
        //            if (graph.Nodes.Values.Any(n => n.GridPosition == pos))
        //            {
        //                _eventBus.Publish(new LogWarning(LOG_TAG,$"Cannot generate room at {pos}"));
        //                continue;
        //            }

        //            var tpl = _catalog.GetRandomRoom();
        //            if (!tpl)
        //            {
        //                _eventBus.Publish(new LogWarning(LOG_TAG,$"Cannot get template for room"));
        //                continue;
        //            }

        //            var node = new RoomNode(tpl, pos);

        //            from.Connections.Add(node.Id);

        //            graph.Nodes[node.Id] = node;
        //            frontier.Add(node);
        //            created = node;
        //            _eventBus.Publish(new Log(LOG_TAG,$"Room generated {node.Id} to {tpl.SceneName}"));
        //            break;
        //        }
        //        var end = new RoomNode(_catalog.EndRoom, Vector2Int.zero);
        //        graph.Nodes[end.Id] = end;
        //        if (created == null) break;
        //    }

        //    _eventBus.Publish(new Log(LOG_TAG,$"{JsonUtility.ToJson(graph)}"));
        //    return graph;
        //}

        public RoomGraph Generate()
        {
            var layout = BuildLayout();
            var graph = new RoomGraph();
            List<RoomNode> prev = null;

            for (var y = 0; y < layout.Count; y++)
            {
                var layer = new List<RoomNode>();
                for (var x = 0; x < layout[y].Length; x++)
                {
                    var type = layout[y][x];
                    var tpl = _catalog.GetRandomRoom(type);
                    if (!tpl)
                        _eventBus.Publish(new LogError(LOG_TAG, "RoomCatalog dont have this room type!"));

                    var node = new RoomNode(tpl, new Vector2Int(x, y));
                    graph.Nodes[node.Id] = node;
                    layer.Add(node);
                }

                if (prev != null)
                    foreach (var p in prev)
                        foreach (var n in layer)
                            p.Connections.Add(n.Id);

                graph.Layers.Add(layer);
                prev = layer;
            }

            graph.StartNodeId = graph.Layers[0][0].Id;
            _eventBus.Publish(new Log(LOG_TAG, "\n" + graph.Dump()));
            return graph;
        }

        private List<RoomType[]> BuildLayout()
        {
            var choices = new List<RoomType>();
            AddMany(choices, RoomType.Altar, GetRandomCountFromRange(_template.AltarCount));
            AddMany(choices, RoomType.ArcanaArena, GetRandomCountFromRange(_template.ArcanaCount));
            AddMany(choices, RoomType.GoldArena, GetRandomCountFromRange(_template.GoldCount));
            AddMany(choices, RoomType.Elite, GetRandomCountFromRange(_template.EliteCount));
            AddMany(choices, RoomType.Shop, GetRandomCountFromRange(_template.ShopCount));
            choices = choices.OrderBy(_ => Random.value).ToList();

            var layers = new List<RoomType[]>
            {
                new[] { RoomType.Start },
                new[] { RoomType.GoldArena }
            };

            while (choices.Count > 0)
            {
                var roomA = choices[0];
                choices.RemoveAt(0);

                var roomB = RoomType.GoldArena;
                var idx = choices.FindIndex(c => c != roomA);
                if (idx >= 0)
                {
                    roomB = choices[idx];
                    choices.RemoveAt(idx);
                }

                layers.Add(new[] { roomA, roomB });
                layers.Add(new[] { RoomType.GoldArena });
            }

            layers.Add(new[] { RoomType.Hub });
            layers.Add(new[] { RoomType.Boss });

            //For target GoldenArena count
            var goldTarget = GetRandomCountFromRange(_template.GoldCount);
            while (layers.Sum(l => l.Count(r => r == RoomType.GoldArena)) < goldTarget)
                InsertAfterSingle(layers, RoomType.GoldArena, layers.Count - 3);

            //Second HUB
            if (GetRandomCountFromRange(_template.HubCount) > 1)
                InsertAfterSingle(layers, RoomType.Hub, layers.Count - 4);

            return layers;
        }

        private static void InsertAfterSingle(List<RoomType[]> layers, RoomType type, int maxIndex)
        {
            var candidates = Enumerable.Range(1, Mathf.Max(0, maxIndex))
                .Where(i => layers[i].Length == 1).ToList();
            if (candidates.Count == 0) return;

            var i = candidates[Random.Range(0, candidates.Count)];
            layers.Insert(i + 1, new[] { type });
        }

        private static void AddMany(List<RoomType> list, RoomType type, int count)
        {
            for (var i = 0; i < count; i++) list.Add(type);
        }

        private static int GetRandomCountFromRange(Vector2Int range) => Random.Range(range.x, range.y + 1);
    }
}
