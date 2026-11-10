using Incision.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Incision.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Inject] private IEventBus _eventBus;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _settingsButton;
        public Button StartButton => _startButton;
        public Button ExitButton => _exitButton;
        public Button SettingsButton => _settingsButton;
    }
}