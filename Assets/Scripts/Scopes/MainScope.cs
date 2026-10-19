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
        }
    }
}
