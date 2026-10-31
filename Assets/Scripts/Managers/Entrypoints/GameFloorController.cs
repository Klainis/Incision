
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Incision.Extensions;
using Incision.Interfaces;
using Incision.Structs.Events;
using VContainer;
using VContainer.Unity;

namespace Incision.Managers
{
    public class GameFloorController : IAsyncStartable, ITaggable
    {
        [Inject] private Enemies.Components.EnemySpawner _enemySpawner;
        [Inject] private IEventBus _eventBus;

        public async Task StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            await _enemySpawner.StartSpawning();
            _eventBus.Publish(new Log(LOG_TAG,"All Enemies Spawned"));
        }

        public string LOG_TAG { get; } = "[GameFloorController]".AsTag(Color.Aqua.ToArgb().ToString());
    }
}
