using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Incision.Enemies.Components;
using Incision.Extensions;
using Incision.Interfaces;
using Incision.Portal.Components;
using Incision.Room.Components;
using Incision.Room.Runtime;
using Incision.Structs.Events;
using Incision.Treasures.Events;
using VContainer;
using VContainer.Unity;
using static UnityEngine.EventSystems.EventTrigger;
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
        private LocationLoader _locationLoader;
        private IEventBus _eventBus;
        private bool _cleared;

        public RoomController(
            IEventBus eventBus, 
            EnemySpawner enemySpawner, 
            PortalSpawner portalSpawner, 
            PortalLinkRegistry portalLinkRegistry,
            LocationLoader locationLoader,
            RoomNode roomNode, 
            RoomView view)
        {
            _eventBus = eventBus;
            _enemySpawner = enemySpawner;
            _portalSpawner = portalSpawner;
            _node = roomNode;
            _linkRegistry = portalLinkRegistry;
            _view = view;
            _locationLoader = locationLoader;
            _eventBus.Subscribe<OnEnemyCountChanged>(OnEnemyCountChanged);
        }

        public async Task StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            AssignPortals();

            if (_node.Template.Type == RoomType.MoneyArena ||
                _node.Template.Type == RoomType.ArcanaArena ||
                _node.Template.Type == RoomType.Elite ||
                _node.Template.Type == RoomType.Boss)
            {
                await _enemySpawner.StartSpawning();
                _eventBus.Publish(new Log(LOG_TAG, "All Enemies Spawned"));
            }

            if (_node.Template.Type == RoomType.Start ||
                _node.Template.Type == RoomType.Shop ||
                _node.Template.Type == RoomType.Hub)
            {
                RegisterPortals();
            }
        }

        private void AssignPortals()
        {
            var connetions = _node.Connections; // ID

            _eventBus.Publish(new Log(LOG_TAG, $"Room: {_node.Template.SceneName.AsTag(Color.White.ToArgb().ToString())}; \n " +
                $"Connections: {string.Join(",", connetions)}; \n" +
                $"ID: {_node.Id.AsTag(Color.White.ToArgb().ToString())}"));

            for (var i = 0; i < _view.Portals.Length; i++)
            {
                var portalEntry = _view.Portals[i];
                var use = i < connetions.Count;
            
                portalEntry.TargetSceneID = use ? connetions[i] : null;
            }
        }

        private void OnEnemyCountChanged(OnEnemyCountChanged evt)
        {
            _eventBus.Publish(new Log(LOG_TAG, evt.Message));

            if (_cleared || evt.Spawner != _enemySpawner || !evt.WavesFinished || evt.CurrentEnemy != 0)
            {
                _eventBus.Publish(new Log(LOG_TAG, $"Cleared: {_cleared}, \n " +
                    $"Not this Spawner: {evt.Spawner != _enemySpawner}, \n " +
                    $"Wave not Finished: {!evt.WavesFinished}, \n" +
                    $"Enemy alive: {evt.CurrentEnemy}"));
                return;
            }
            _cleared = true;
            
            if (_node.Template.Type == RoomType.MoneyArena)
                _eventBus.Publish(new SpawnTreasure(_node));

            RegisterPortals();
        }

        private void RegisterPortals()
        {
            var conns = _node.Connections;

            for (var i = 0; i < _view.Portals.Length; i++)
            {
                var use = i < conns.Count;
                if (!use) break;

                var portalEntry = _view.Portals[i];
                var target = string.IsNullOrEmpty(portalEntry.TargetSceneID) ? null : _locationLoader.GetNode(portalEntry.TargetSceneID);
                var link = new PortalLink
                {
                    SourceRoomId = _node.Id,
                    Anchor = portalEntry.Anchor,
                    TargetSceneID = portalEntry.TargetSceneID,
                    Icon = target.Template.PortalIcon
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
