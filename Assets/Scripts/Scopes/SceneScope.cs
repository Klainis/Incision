using Incision.Enemies.Runtime;
using Incision.Enemies.Services;
using Incision.PLayer;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Incision.Scopes
{
    public class SceneScope : LifetimeScope
    {
        [SerializeField] private Player _player;
        [SerializeField] private UIEffectManager uiEffectManager;
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("<color=white>[DI]</color> Scene scope configuration started");
            builder.RegisterComponent(_player);

            builder.Register<NearestPlayerTargetProvider>(Lifetime.Singleton)
                .WithParameter(_player.transform)
                .AsImplementedInterfaces();
            builder.Register<NavMeshService>(Lifetime.Scoped).AsImplementedInterfaces();
            
            builder.Register<EnemyFactory>(Lifetime.Singleton); 
            builder.RegisterComponent(uiEffectManager); 
            builder.Register<ProjectileFactory>(Lifetime.Singleton);
            builder.Register<ObjectFactory>(Lifetime.Singleton);
        }
    }
}
