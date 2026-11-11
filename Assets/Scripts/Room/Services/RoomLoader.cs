using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Incision.Interfaces;
using Incision.Managers;
using Incision.Room.Runtime;
using Incision.Room.Services.Interfaces;
using Incision.Structs.Events;
using UnityEngine.SceneManagement;
using VContainer;

namespace Incision.Room.Services
{
    public class RoomLoader : IRoomLoader
    {
        [Inject] private IEventBus _eventBus;
        private readonly Dictionary<string, RoomNode> _loaded = new();
        private readonly SemaphoreSlim _lock = new(1, 1);
        public bool IsLoaded(string nodeId) => _loaded.ContainsKey(nodeId);

        public async Task LoadAsync(RoomNode node, LoadSceneMode mode = LoadSceneMode.Additive)
        {
            if (_loaded.ContainsKey(node.Id)) 
                throw new TypeLoadException("Room already loaded");
            await _lock.WaitAsync();
            
            try
            {
                PendingRoomNode.Set(node);
                await SceneLoader.LoadScene(node.Template.SceneName, LoadSceneMode.Single);

                _loaded[node.Id] = node;
            }
            finally
            {
                _eventBus.Publish(new Log("[RoomLoader]", $"Welcome to {node.Id}"));
                _lock.Release();
            }
        }

        public async Task UnloadAsync(string nodeId)
        {
            if (!_loaded.TryGetValue(nodeId, out var node)) return;
            await _lock.WaitAsync();
            try
            {
                await SceneLoader.UnloadScene(node.Template.SceneName);

                _loaded.Remove(nodeId);
            }
            finally { _lock.Release(); }
        }
    }
}
