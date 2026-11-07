using Incision.Effects;
using Incision.Managers;
using Incision.Room.Runtime;
using Incision.Room.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Incision.Scopes
{
    public class MainScope : LifetimeScope
    {
        [Header("Logging")]
        [SerializeField] private bool LoggingEnabled = true;
        [Header("Effects")]
        [SerializeField] private DecalBufferTemplate decalBufferTemplate;
        
        [SerializeField] private RoomGraphTemplate roomGraphTemplate;
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.unityLogger.logEnabled = LoggingEnabled;
            
            Debug.Log($"<color=white>[DI]</color> Main scope configuration started");
            builder.Register<EventBus>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .AsSelf();
            builder.Register<DecalBuffer>(Lifetime.Singleton)
                .WithParameter(decalBufferTemplate)
                .AsImplementedInterfaces()
                .AsSelf();
            
            builder.RegisterInstance(new LocationConfig { CellSize = 60f });
            builder.RegisterInstance(roomGraphTemplate);
            
            builder.Register<PortalLinkRegistry>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<RoomGraphGenerator>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<LocationLoader>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<RoomLoader>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            builder.RegisterEntryPoint<GameController>();
        }
    }
}
