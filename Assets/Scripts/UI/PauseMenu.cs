
using Incision.Arcana;
using Incision.Events;
using Incision.Interfaces;
using Incision.Managers;
using UnityEngine;
using VContainer;

namespace Incision.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseMenu;
        
        [Inject] private PauseManager _pauseManager;
        [Inject] private CursorManager _cursorManager;
        [Inject] private IEventBus _eventBus;

        private ArcanaRewardController _arcanaReward;

        [Inject]
        private void SetArcanaReward(ArcanaRewardController arcanaReward) => _arcanaReward = arcanaReward;
        
        private void Start()
        {
            _pauseMenu.SetActive(false);
        }

        private void Update()
        {
            if (_arcanaReward?.IsOpen == true || !Input.GetKeyDown(KeyCode.Escape))
                return;
            
            if (_pauseManager.IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        public void Pause()
        {
            if (_arcanaReward?.IsOpen == true)
                return;

            _pauseManager.Pause();
            _pauseMenu.SetActive(true);
            _cursorManager.ShowCursor();
        }

        public void Resume()
        {
            if (_arcanaReward?.IsOpen == true)
                return;

            _pauseManager.Resume();
            _pauseMenu.SetActive(false);
            _cursorManager.HideCursor();
        }

        public void OnMainMenu()
        {
            _eventBus.Publish(new OnGameFinish(true, "Return to menu"));
        }
    }
}
