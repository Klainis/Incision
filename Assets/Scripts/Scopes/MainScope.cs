using Incision.Effects;
using Incision.Arcana;
using Incision.Managers;
using Incision.Room.Services;
using Incision.Templates;
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
        [SerializeField] private GameControllerSettings gameControllerSettings;
        [SerializeField] private ArcanaDefinition[] _arcanaDefinitions;
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
            builder.Register<PauseManager>(Lifetime.Singleton);
            builder.Register<CursorManager>(Lifetime.Singleton);
            builder.Register<GoldManager>(Lifetime.Singleton);
            builder.Register<ArcanaCatalog>(Lifetime.Singleton).WithParameter(_arcanaDefinitions);
            builder.Register<ArcanaLoadout>(Lifetime.Singleton);
            builder.Register<ArcanaRewardController>(Lifetime.Singleton);
            builder.RegisterBuildCallback(resolver => resolver.Resolve<ArcanaRewardController>());
            
            builder.RegisterInstance(new LocationConfig { CellSize = 60f });
            builder.RegisterInstance(roomGraphTemplate);
            builder.RegisterInstance(gameControllerSettings);
            
            builder.Register<GameController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            
            builder.Register<PortalLinkRegistry>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<RoomGraphGenerator>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<LocationLoader>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<RoomLoader>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        }
    }
}
