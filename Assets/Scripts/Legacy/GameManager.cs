
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Incision.Legacy
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Scenes")]
        [Scene]
        [SerializeField] private string _startScene;

        [Scene]
        [SerializeField] private string _mainMenuScene;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SetCursorVisible(true);
            Time.timeScale = 1f;
        }

        public void OnStartGame()
        {
            Time.timeScale = 1f;
            SetCursorVisible(false);

            SceneManager.LoadScene(_startScene);
        }

        public void OnOpenMainMenu()
        {
            Time.timeScale = 1f;
            SetCursorVisible(true);

            SceneManager.LoadScene(_mainMenuScene);
        }

        public void OnExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void SetCursorVisible(bool visible)
        {
            Cursor.visible = visible;
            Cursor.lockState = visible
                ? CursorLockMode.None
                : CursorLockMode.Locked;
        }
    }
}
