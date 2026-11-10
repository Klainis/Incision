using Incision.Events;
using Incision.Extensions;
using Incision.Interfaces;
using UnityEngine;
using VContainer;

namespace Incision.Managers
{
    public class PauseManager : ITaggable
    {
        private bool _paused = false;
        public bool IsPaused => _paused;

        [Inject] private IEventBus _eventBus;

        private void SetPaused(bool paused)
        {
            Time.timeScale = paused ? 0f : 1f;
            _paused = paused;
            
            _eventBus.Publish(new OnPauseChanged(IsPaused));
        }
        
        public void Resume()
        {
            if (!_paused)
                return;
            
            SetPaused(false);
        }
        
        public void Pause()
        {
            if (_paused)
                return;
            
            SetPaused(true);
        }

        public string LOG_TAG { get; } = "[PauseManager]".AsTag(Color.yellow);
    }
}
