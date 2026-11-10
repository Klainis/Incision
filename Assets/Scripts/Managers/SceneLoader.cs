using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Incision.Managers
{
    public class SceneLoader : MonoBehaviour
    {
        public static async Task LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Additive)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName, mode);
            asyncOperation!.allowSceneActivation = false;
            while (asyncOperation.progress < 0.9f)
            {
                await Task.Yield();
            }
            asyncOperation.allowSceneActivation = true; 
        }
        
        public static async Task UnloadScene(string sceneName)
        {
            var asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
            asyncOperation!.allowSceneActivation = false;
            while (asyncOperation.progress < 0.9f)
            {
                await Task.Yield();
            }
            asyncOperation.allowSceneActivation = true;
        }
    }
}
