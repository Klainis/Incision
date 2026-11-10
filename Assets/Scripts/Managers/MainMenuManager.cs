using System;
using Incision.Interfaces;
using Incision.UI;
using Incision.Structs.Events;
using Incision.Templates;
using VContainer;
using VContainer.Unity;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Incision.Managers
{
    public class MainMenuManager : IStartable, IDisposable
    {
        [Inject] private CursorManager _cursorManager;
        [Inject] private PauseManager _pauseManager;
        [Inject] private MainMenuUI _mainMenuUI;
        [Inject] private IEventBus _eventBus;
        [Inject] private GameController _gameController;
        [Inject] private GameControllerSettings _settings;
        
        public void Start()
        {
            _cursorManager.ShowCursor();
            _pauseManager.Resume();
            _mainMenuUI.StartButton.onClick.AddListener(OnStartGame);
            _mainMenuUI.ExitButton.onClick.AddListener(OnExitGame);
            _mainMenuUI.SettingsButton.onClick.AddListener(OnShowSettings);
        }
        
        public void Dispose()
        {
            _mainMenuUI.StartButton.onClick.RemoveListener(OnStartGame);
            _mainMenuUI.ExitButton.onClick.RemoveListener(OnExitGame);
            _mainMenuUI.SettingsButton.onClick.RemoveListener(OnShowSettings);
        }

        private async void OnStartGame()
        {
            try
            {
                _pauseManager.Resume();
                _cursorManager.HideCursor();
                
                await _gameController.StartGame();
            }
            catch (Exception e)
            {
                _eventBus.Publish(new LogError("[MainMenuManager]", e.Message));
            }
        }
        
        private void OnShowSettings()
        {
            _eventBus.Publish(new LogWarning("[MainMenuManager]","Settings are not implemented"));
        }

        private void OnExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
