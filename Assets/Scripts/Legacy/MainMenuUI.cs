using UnityEngine;
using UnityEngine.UI;

namespace Incision.Legacy
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _exitButton;

        private void Start()
        {
            _startButton.onClick.AddListener(GameManager.Instance.OnStartGame);
            _exitButton.onClick.AddListener(GameManager.Instance.OnExitGame);
        }

        private void OnDestroy()
        {
            if (GameManager.Instance == null)
                return;

            _startButton.onClick.RemoveListener(GameManager.Instance.OnStartGame);
            _exitButton.onClick.RemoveListener(GameManager.Instance.OnExitGame);
        }
    }
}