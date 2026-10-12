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
        var selectedPrefab = isCrit ? _critDamagePopupPrefab : _baseDamagePopupPrefab;
        _popupInstance = Instantiate(selectedPrefab, _damageCanvas.transform);
        _popupInstance.SetDamageText(worldPosition, damage);
    }
}
