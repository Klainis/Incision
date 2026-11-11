using System;
using Incision.Enemies.Components;
using Incision.Enemies.Runtime;
using Incision.Enemies.Services;
using Incision.Extensions;
using Incision.Portal;
using Incision.Portal.Components;
using Incision.Room.Components;
using Incision.Room.Runtime;
using Incision.Room.Services;
using Incision.Room.Templates;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Incision.Scopes
{
    public class SceneScope : LifetimeScope
    {
        [Header("Test Room Template")]
        [SerializeField] private RoomTemplate _debugRoomTemplate;

        [Space(5)]
        [SerializeField] private Player _player;
        [SerializeField] private RoomView _view;
        [SerializeField] private UIEffectManager uiEffectManager;
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private PortalSpawner portalSpawner;
        
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("<color=white>[DI]</color> Scene scope configuration started");
            builder.RegisterInstance(ResolveRoomNode());
            
            builder.Register<NearestPlayerTargetProvider>(Lifetime.Singleton)
                .WithParameter(_player.transform)
                .AsImplementedInterfaces();
            builder.Register<NavMeshService>(Lifetime.Scoped).AsImplementedInterfaces();
            
            builder.Register<EnemyFactory>(Lifetime.Singleton); 
            builder.Register<ProjectileFactory>(Lifetime.Singleton);
            builder.Register<ObjectFactory>(Lifetime.Singleton);
            builder.Register<PortalFactory>(Lifetime.Singleton);

            builder.RegisterComponent(_view);
            builder.RegisterComponent(uiEffectManager); 
            builder.RegisterComponent(enemySpawner);
            builder.RegisterComponent(portalSpawner);
            
            builder.RegisterEntryPoint<RoomController>();
        }

        private RoomNode ResolveRoomNode()
        {
            if (PendingRoomNode.TryConsume(out var node))
                return node;

#if UNITY_EDITOR
            if (_debugRoomTemplate != null)
            {
                Debug.LogWarning(
                    $"[SceneScope] PendingRoomNode is Empty, using debug node {_debugRoomTemplate.name}");
                return new RoomNode(_debugRoomTemplate, Vector2Int.zero);
            }
#endif

            throw new InvalidOperationException("Loaded room is not valid".AsTag(Color.red));
        }
    }
}
