using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Incision.Scopes
{
    public class MainScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log($"<color=purple>[DI]</color> Main scope configuration started");
            builder.Register<EventBus>(Lifetime.Singleton).AsImplementedInterfaces();
            
        }
    }
}
