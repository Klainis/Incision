using System;
using System.Threading;
using System.Threading.Tasks;
using Incision.Enemies.Components;
using Incision.Extensions;
using Incision.Interfaces;
using Incision.Portal.Components;
using Incision.Room.Components;
using Incision.Room.Runtime;
using Incision.Structs.Events;
using VContainer.Unity;
using Color = System.Drawing.Color;

namespace Incision.Room.Services
{
    public class RoomController : IAsyncStartable, ITaggable, IDisposable
    {
        public string LOG_TAG { get; } = "[GameFloorController]".AsTag(Color.Aqua.ToArgb().ToString());
        
        private RoomNode _node;
        private RoomView _view;
        private PortalSpawner _portalSpawner;
        private PortalLinkRegistry _linkRegistry;
        private EnemySpawner _enemySpawner;
        private IEventBus _eventBus;

        public RoomController(
            IEventBus eventBus, 
            EnemySpawner enemySpawner, 
            PortalSpawner portalSpawner, 
            PortalLinkRegistry portalLinkRegistry,
            RoomNode roomNode, 
            RoomView view)
        {
            _eventBus = eventBus;
            _enemySpawner = enemySpawner;
            _portalSpawner = portalSpawner;
            _node = roomNode;
            _linkRegistry = portalLinkRegistry;
            _view = view;
            _eventBus.Subscribe<OnEnemyCountChanged>(OnEnemyCountChanged);
        }

        public async Task StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            AssignPortals();
            
            await _enemySpawner.StartSpawning();
            _eventBus.Publish(new Log(LOG_TAG,"All Enemies Spawned"));
        }

        private void AssignPortals()
        {
            var conns = _node.Connections;
            
            for (var i = 0; i < _view.Portals.Length; i++)
            {
                var entry = _view.Portals[i];
                var used = i < conns.Count;
            
                entry.TargetSceneName = used ? conns[i] : null;
            }
        }

        private void OnEnemyCountChanged(OnEnemyCountChanged evt)
        {
            _eventBus.Publish(new Log(LOG_TAG, evt.Message));
            if (evt.Current > 0) return;
            
            foreach (var entry in _view.Portals)
            {
                var link = new PortalLink
                {
                    SourceRoomId = _node.Id,
                    Anchor = entry.Anchor,
                    TargetSceneName = entry.TargetSceneName
                };
                _linkRegistry.Register(link);
                _portalSpawner.SpawnPortal(link);
            }
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<OnEnemyCountChanged>(OnEnemyCountChanged);
            _linkRegistry.UnregisterRoom(_node.Id);
        }
    }
}
