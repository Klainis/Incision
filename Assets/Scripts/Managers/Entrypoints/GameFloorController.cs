using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Incision.Enemies.Components;
using Incision.Extensions;
using Incision.Interfaces;
using Incision.Portal.Components;
using Incision.Portal.Structs.Events;
using Incision.Structs.Events;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Incision.Managers
{
    public class GameFloorController : IAsyncStartable, ITaggable
    {
        private PortalSpawner _portalSpawner;
        private EnemySpawner _enemySpawner;
        private IEventBus _eventBus;

        public GameFloorController(IEventBus eventBus, IObjectResolver resolver, EnemySpawner enemySpawner, PortalSpawner portalSpawner)
        {
            _eventBus = eventBus;
            _enemySpawner = enemySpawner;
            _portalSpawner = portalSpawner;
            _eventBus.Subscribe<OnEnemyCountChanged>(OnEnemyCountChanged);
            _eventBus.Subscribe<OnPortalEntered>(OnPortalEntered);
        }

        public async Task StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            await _enemySpawner.StartSpawning();
            _eventBus.Publish(new Log(LOG_TAG,"All Enemies Spawned"));
        }

        private void OnEnemyCountChanged(OnEnemyCountChanged evt)
        {
            _eventBus.Publish(new Log(LOG_TAG, evt.Message));
            if (evt.Current > 0) return;

            _portalSpawner.SpawnPortal();
        }
        
        private void OnPortalEntered(OnPortalEntered evt)
        {
            _eventBus.Publish(new Log(LOG_TAG, evt.Message));

            SceneManager.LoadScene("TestScene");
        }

        public string LOG_TAG { get; } = "[GameFloorController]".AsTag(Color.Aqua.ToArgb().ToString());
    }
}
