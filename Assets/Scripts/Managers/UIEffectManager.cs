using UnityEngine;

public class UIEffectManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Canvas _damageCanvas;
    [SerializeField] private DamagePopup _popupPrefab;

    private DamagePopup _popupInstance;

    public void ShowDamagePopup(Vector3 worldPosition, float damage)
    {
        if (_popupPrefab == null || _damageCanvas == null)
        {
            Debug.LogWarning("UIEffectManager: �� ��������� ������ ��� Canvas!");
            return;
        }

        _popupInstance = Instantiate(_popupPrefab, _damageCanvas.transform);
        _popupInstance.SetDamageText(worldPosition, damage);
    }
}
