using System;
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
            _template = template;
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
            Debug.Log($"template={(_template != null)}, catalog={(_catalog != null)}");

            var layout = BuildLayout();
            var graph = new RoomGraph();
            List<RoomNode> nodesOnOldLayer = null;

            for (var y = 0; y < layout.Count; y++) // for layout 
            {
                var nodesOnNewLayer = new List<RoomNode>(); // nodesOnNewLayer

                for (var x = 0; x < layout[y].Length; x++) // for room in nodesOnNewLayer
                {
                    var type = layout[y][x];
                    var tpl = _catalog.GetRandomRoom(type);
                    if (!tpl)
                        throw new NullReferenceException($"{LOG_TAG} RoomCatalog dont have this room type: {type.ToString().AsTag(Color.white)}");

                    var node = new RoomNode(tpl, new Vector2Int(x, y)); // room of _type
                    graph.Nodes[node.Id] = node;
                    nodesOnNewLayer.Add(node);
                }

                if (nodesOnOldLayer != null)
                {
                    foreach (var oldNode in nodesOnOldLayer)
                        foreach (var newNode in nodesOnNewLayer)
                            oldNode.Connections.Add(newNode.Id);
                }

                graph.Layers.Add(nodesOnNewLayer);
                nodesOnOldLayer = nodesOnNewLayer;
            }

            graph.StartNodeId = graph.Layers[0][0].Id;
            _eventBus.Publish(new Log(LOG_TAG, "\n" + graph.Dump()));
            return graph;
        }

        private List<RoomType[]> BuildLayout()
        {
            var pool = new List<RoomType>();
            AddMany(pool, RoomType.Altar, GetRandomCountFromRange(_template.AltarCount));
            AddMany(pool, RoomType.ArcanaArena, GetRandomCountFromRange(_template.ArcanaCount));
            AddMany(pool, RoomType.Elite, GetRandomCountFromRange(_template.EliteCount));
            AddMany(pool, RoomType.Shop, GetRandomCountFromRange(_template.ShopCount));

            var moneyCount = Mathf.Max(0, GetRandomCountFromRange(_template.GoldCount) - 1);
            AddMany(pool, RoomType.MoneyArena, moneyCount);

            pool = pool.OrderBy(_ => UnityEngine.Random.value).ToList();

            var layers = new List<RoomType[]>
            {
                new[] { RoomType.Start },
                new[] { RoomType.MoneyArena }
            };

            while (pool.Count > 0)
            {
                //var roomA = pool[0];
                //pool.RemoveAt(0);

                //var roomB = RoomType.MoneyArena;
                //var idx = pool.FindIndex(c => c != roomA);
                //if (idx >= 0)
                //{
                //    roomB = pool[idx];
                //    pool.RemoveAt(idx);
                //}

                //layers.Add(new[] { roomA, roomB });
                //layers.Add(new[] { RoomType.MoneyArena });

                var roomA = Pop(pool);

                var idx = pool.FindIndex(c => c != roomA);
                if (idx < 0)
                {
                    layers.Add(new[] { roomA });
                    continue;
                }

                var roomB = pool[idx];
                pool.RemoveAt(idx);
                layers.Add(new[] { roomA, roomB });

                if(pool.Count > 0 && UnityEngine.Random.value >= 0.5)
                    layers.Add(new[] { Pop(pool) });
            }

            layers.Add(new[] { RoomType.Hub });
            layers.Add(new[] { RoomType.Boss });

            //For target GoldenArena count
            var goldTarget = GetRandomCountFromRange(_template.GoldCount);
            while (layers.Sum(l => l.Count(r => r == RoomType.MoneyArena)) < goldTarget)
                InsertAfterSingle(layers, RoomType.MoneyArena, layers.Count - 3);

            //Second HUB
            if (GetRandomCountFromRange(_template.HubCount) > 1)
                InsertAfterSingle(layers, RoomType.Hub, layers.Count - 4);

            return layers;
        }

        private static RoomType Pop(List<RoomType> list)
        {
            var item = list[0];
            list.RemoveAt(0);
            return item;
        }

        private static void InsertAfterSingle(List<RoomType[]> layers, RoomType type, int maxIndex)
        {
            var candidates = Enumerable.Range(1, Mathf.Max(0, maxIndex))
                .Where(i => layers[i].Length == 1).ToList();
            if (candidates.Count == 0) return;

            var i = candidates[UnityEngine.Random.Range(0, candidates.Count)];
            layers.Insert(i + 1, new[] { type });
        }

        private static void AddMany(List<RoomType> list, RoomType type, int count)
        {
            for (var i = 0; i < count; i++) list.Add(type);
        }

        private static int GetRandomCountFromRange(Vector2Int range) => UnityEngine.Random.Range(range.x, range.y + 1);
    }
}
