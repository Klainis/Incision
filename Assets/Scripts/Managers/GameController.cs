using System;
using System.Threading.Tasks;
using Incision.Events;
using Incision.Extensions;
using Incision.Interfaces;
using Incision.Portal.Structs.Events;
using Incision.Room.Services;
using Incision.Structs.Events;
using Incision.Templates;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Incision.Managers
{
    public class GameController : ITaggable
    {
        public string LOG_TAG { get; } = "[GameController]".AsTag(Color.yellow);

        [Inject] private GameControllerSettings _settings;
        [Inject] private readonly LocationLoader _loc;
        [Inject] private readonly IEventBus _eventBus;
        [Inject] private readonly GoldManager _goldManager;

        public async Task StartGame()
        {
            _goldManager.Reset();
            _eventBus.Subscribe<OnPortalEntered>(OnPortalEntered);
            _eventBus.Subscribe<OnGameFinish>(FinishGame);
            await _loc.BuildAsync();
        }

        public void FinishGame(OnGameFinish evt)
        {
            _eventBus.Unsubscribe<OnPortalEntered>(OnPortalEntered);
            _eventBus.Unsubscribe<OnGameFinish>(FinishGame);
            SceneManager.LoadScene(_settings.MainScene);
        }
        
        private async void OnPortalEntered(OnPortalEntered evt)
        {
            try
            {
                _eventBus.Publish(new Log(LOG_TAG, evt.Message));
                // TODO: needs a solution for loading with empty scene as starter, currently it breaks lighting
                // await _loc.EnsureRoomUnloaded(evt.PortalLink.SourceRoomId);
                if (!string.IsNullOrEmpty(evt.PortalLink.TargetSceneID))
                {
                    await _loc.EnsureRoomLoaded(evt.PortalLink.TargetSceneID);
                }
                else
                {
                    _eventBus.Publish(new OnGameFinish(false, "End of run"));
                }
            }
            catch (Exception e)
            {
                _eventBus.Publish(new LogError(LOG_TAG, e.Message));
            }
        }
    }
}
