using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIEffectManager : MonoBehaviour
{
    public static UIEffectManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private Canvas _damageCanvas;
    [SerializeField] private DamagePopup _baseDamagePopupPrefab;
    [SerializeField] private DamagePopup _critDamagePopupPrefab;

    private DamagePopup _popupInstance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowDamagePopup(Vector3 worldPosition, float damage, bool isCrit)
    {
        if ((_baseDamagePopupPrefab == null || _critDamagePopupPrefab == null) || _damageCanvas == null)
        {
            Debug.LogWarning("UIEffectManager: Не назначены префаб Popup или Canvas!");
            return;
        }

        _popupInstance = isCrit ? Instantiate(_critDamagePopupPrefab, _damageCanvas.transform) : Instantiate(_baseDamagePopupPrefab, _damageCanvas.transform);
        _popupInstance.SetDamageText(worldPosition, damage);
    }
}
