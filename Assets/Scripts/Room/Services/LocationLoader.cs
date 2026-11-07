using System;
using System.Threading.Tasks;
using Incision.Room.Runtime;
using Incision.Room.Services.Interfaces;
using VContainer;

namespace Incision.Room.Services
{
    public class LocationLoader
    {
        [Inject] private readonly IRoomGraphGenerator _gen;
        [Inject] private readonly IRoomLoader _loader;
        [Inject] private readonly LocationConfig _cfg;

        private RoomGraph _graph;
        public async Task<RoomGraph> BuildAsync(int roomCount)
        {
            _graph = _gen.Generate(roomCount);

            await EnsureRoomLoaded(_graph.StartNodeId);
            return _graph;
        }

        public async Task EnsureRoomLoaded(string roomID)
        {
            if (_loader.IsLoaded(roomID)) 
                return;
            
            if (!_graph.Nodes.TryGetValue(roomID, out var node))
                throw new NullReferenceException($"Graph has no {roomID} node");
                
            await _loader.LoadAsync(node);
        }
        
        public async Task EnsureRoomUnloaded(string roomID)
        {
            if (!_loader.IsLoaded(roomID))
                return;
            if (!_graph.Nodes.TryGetValue(roomID, out var node))
                throw new NullReferenceException($"Graph has no {roomID} node");
                
            await _loader.UnloadAsync(roomID);
        }
    }

    public class LocationConfig { public float CellSize = 60f; }
}
