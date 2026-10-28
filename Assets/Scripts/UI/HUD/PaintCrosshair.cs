using Incision.Enemies.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Incision
{
    public class PaintCrosshair : MonoBehaviour
    {
        [Header("Crosshair")]
        [SerializeField] private Image _crosshairIndicator;
        [SerializeField] private Color _paintColor = Color.red;
        [SerializeField] private Color _normalColor = Color.white;

        [Header("Camera")]
        [SerializeField] private Camera _mainCamera;

        [Header("Collisions")]
        [SerializeField] private LayerMask _enemyLayers;

        private void Start()
        {
            _crosshairIndicator.color = _normalColor;
        }

        private void LateUpdate()
        {
            Ray ray = _mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            GameObject target = Physics.Raycast(ray, out RaycastHit hit) ? hit.collider.gameObject : null;

            bool isEnemy = target ? (_enemyLayers.value & (1 << target.layer)) != 0 : false;
            Paint(isEnemy);
        }

        private void Paint(bool isEnemy)
        {
            _crosshairIndicator.color = isEnemy ? _paintColor : _normalColor;
        }
    }
}
