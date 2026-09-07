using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIEffectManager : MonoBehaviour
{
    public static UIEffectManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private Canvas _damageCanvas;
    [SerializeField] private DamagePopup _popupPrefab;

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

    public void ShowDamagePopup(Vector3 worldPosition, float damage)
    {
        if (_popupPrefab == null || _damageCanvas == null)
        {
            Debug.LogWarning("UIEffectManager: Не назначены префаб или Canvas!");
            return;
        }

        _popupInstance = Instantiate(_popupPrefab, _damageCanvas.transform);
        _popupInstance.SetDamageText(worldPosition, damage);
    }
}
