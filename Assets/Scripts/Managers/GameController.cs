using System;
using System.Threading;
using System.Threading.Tasks;
using Incision.Extensions;
using Incision.Interfaces;
using Incision.Portal.Structs.Events;
using Incision.Room.Services;
using Incision.Structs.Events;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Incision.Managers
{
    public class GameController : IAsyncStartable, ITaggable
    {
        [Inject] private readonly LocationLoader _loc;
        [Inject] private readonly IEventBus _eventBus;

        public async Task StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            _eventBus.Subscribe<OnPortalEntered>(OnPortalEntered);
            await _loc.BuildAsync(roomCount: 8);
        }
        
        private async void OnPortalEntered(OnPortalEntered evt)
        {
            _eventBus.Publish(new Log(LOG_TAG, evt.Message));
            await _loc.EnsureRoomUnloaded(evt.PortalLink.SourceRoomId);
            if (!string.IsNullOrEmpty(evt.PortalLink.TargetSceneName))
                await _loc.EnsureRoomLoaded(evt.PortalLink.TargetSceneName);
            else
            {
                _eventBus.Publish(new Log(LOG_TAG, $"That's all folks!".AsTag(Color.magenta)));
            }
        }

        public string LOG_TAG { get; } = "[GameController]".AsTag(Color.yellow);
    }
}
