using System;
using Incision.Enemies.Components;
using Incision.Enemies.Runtime;
using Incision.Enemies.Services;
using Incision.Portal;
using Incision.Portal.Components;
using Incision.Room.Components;
using Incision.Room.Runtime;
using Incision.Room.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Incision.Scopes
{
    public class SceneScope : LifetimeScope
    {
        [SerializeField] private Player _player;
        [SerializeField] private RoomView _view;
        [SerializeField] private UIEffectManager uiEffectManager;
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private PortalSpawner portalSpawner;
        
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("<color=white>[DI]</color> Scene scope configuration started");
            var pendingNode = PendingRoomNode.Value;

            if (pendingNode == null)
                throw new Exception($"{pendingNode} is not a valid room node");
            else
                builder.RegisterInstance(pendingNode);
            
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
;        }
    }
}
