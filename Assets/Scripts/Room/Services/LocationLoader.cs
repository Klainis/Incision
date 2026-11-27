using System;
using System.Threading.Tasks;
using Incision.Room.Runtime;
using Incision.Room.Services.Interfaces;
using UnityEngine.SceneManagement;
using VContainer;

namespace Incision.Room.Services
{
    public class LocationLoader
    {
        [Inject] private readonly IRoomGraphGenerator _graphGenerator;
        [Inject] private readonly IRoomLoader _loader;
        [Inject] private readonly LocationConfig _cfg;

        private RoomGraph _graph;
        public async Task<RoomGraph> BuildAsync()
        {
            _graph = _graphGenerator.Generate();

            await EnsureRoomLoaded(_graph.StartNodeId, LoadSceneMode.Single);
            return _graph;
        }

        public async Task EnsureRoomLoaded(string roomID, LoadSceneMode mode = LoadSceneMode.Additive)
        {
            if (_loader.IsLoaded(roomID)) 
                return;
            
            if (!_graph.Nodes.TryGetValue(roomID, out var node))
                throw new NullReferenceException($"Graph has no {roomID} node. \n Scene: {node.Template.SceneName}");
                
            await _loader.LoadAsync(node, mode);
        }
        
        public async Task EnsureRoomUnloaded(string roomID)
        {
            if (!_loader.IsLoaded(roomID))
                return;
            if (!_graph.Nodes.TryGetValue(roomID, out var node))
                throw new NullReferenceException($"Graph has no {roomID} node. \n Scene: {node.Template.SceneName}");
                
            await _loader.UnloadAsync(roomID);
        }
    }

    public class LocationConfig { public float CellSize = 60f; }
}
