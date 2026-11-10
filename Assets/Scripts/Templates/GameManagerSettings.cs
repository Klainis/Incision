using UnityEngine;

namespace Incision.Templates
{
    [CreateAssetMenu(fileName = "GameControllerSettings", menuName = "General/Managers/GameSettings", order = 1)]
    public class GameControllerSettings : ScriptableObject
    {
        [SerializeField, Scene] private string mainScene;
        [SerializeField, Scene] private string emptyScene;
        
        public string MainScene => mainScene;
        public string EmptyScene => emptyScene;
    }
}
