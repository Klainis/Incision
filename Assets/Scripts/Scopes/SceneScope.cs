using Incision.Enemies.Components;
using Incision.Enemies.Runtime;
using Incision.Enemies.Services;
using Incision.Managers;
using Incision.Portal;
using Incision.Portal.Components;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Incision.Scopes
{
    public class SceneScope : LifetimeScope
    {
        [SerializeField] private PlayerInput player;
        [SerializeField] private UIEffectManager uiEffectManager;
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private PortalSpawner portalSpawner;
        
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("<color=white>[DI]</color> Scene scope configuration started");
            builder.Register<NearestPlayerTargetProvider>(Lifetime.Singleton)
                .WithParameter(player.transform)
                .AsImplementedInterfaces();
            builder.Register<NavMeshService>(Lifetime.Scoped).AsImplementedInterfaces();
            
            builder.Register<EnemyFactory>(Lifetime.Singleton);
            builder.Register<PortalFactory>(Lifetime.Singleton);
            
            builder.RegisterComponent(enemySpawner);
            builder.RegisterComponent(portalSpawner);
            builder.RegisterComponent(uiEffectManager);
            
            builder.RegisterEntryPoint<GameFloorController>();
;        }
    }
}
