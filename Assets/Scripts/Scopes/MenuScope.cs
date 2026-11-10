using Incision.Managers;
using Incision.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Incision.Scopes
{
    public class MenuScope : LifetimeScope
    {
        [SerializeField] private MainMenuUI menu;
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("<color=white>[DI]</color> Menu scope configuration started");
            builder.RegisterInstance(menu);
            
            builder.RegisterEntryPoint<MainMenuManager>();
;        }
    }
}
