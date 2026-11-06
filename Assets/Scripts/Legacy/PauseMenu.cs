
using UnityEngine;

namespace Incision.Legacy
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseMenu;

        private bool _isPaused;

        private void Start()
        {
            _pauseMenu.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isPaused)
                    OnResume();
                else
                    OnOpenMenu();
            }
        }

        public void OnOpenMenu()
        {
            _isPaused = true;

            _pauseMenu.SetActive(true);
            Time.timeScale = 0f;

            GameManager.Instance.SetCursorVisible(true);
        }

        public void OnResume()
        {
            _isPaused = false;

            _pauseMenu.SetActive(false);
            Time.timeScale = 1f;

            GameManager.Instance.SetCursorVisible(false);
        }

        public void OnMainMenu()
        {
            _isPaused = false;

            GameManager.Instance.OnOpenMainMenu();
        }
    }
}
