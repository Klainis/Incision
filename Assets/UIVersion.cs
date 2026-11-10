using TMPro;
using UnityEngine;

namespace Incision
{
    public class UIVersion : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI versionText;

        void Start()
        {
            versionText.text = $"V{Application.version}";
        }
    }
}
