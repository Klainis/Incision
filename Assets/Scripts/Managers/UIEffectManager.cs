using UnityEngine;

public class UIEffectManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Canvas _damageCanvas;
    [SerializeField] private DamagePopup _baseDamagePopupPrefab;
    [SerializeField] private DamagePopup _critDamagePopupPrefab;

    private DamagePopup _popupInstance;

    public void ShowDamagePopup(Vector3 worldPosition, float damage, bool isCrit = false)
    {
        if ((_baseDamagePopupPrefab == null || _critDamagePopupPrefab == null) || _damageCanvas == null)
        {
            Debug.LogWarning("UIEffectManager: �� ��������� ������ Popup ��� Canvas!");
            return;
        }

        _popupInstance = isCrit ? Instantiate(_critDamagePopupPrefab, _damageCanvas.transform) : Instantiate(_baseDamagePopupPrefab, _damageCanvas.transform);
        _popupInstance.SetDamageText(worldPosition, damage);
    }
}
