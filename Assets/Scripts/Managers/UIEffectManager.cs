using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

public class UIEffectManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Canvas _damageCanvas;
    [SerializeField] private Canvas _hudCanvas;

    [Header("Blood Screen Effect")]
    [SerializeField] private GameObject _bloodScreenObject;
    [SerializeField] private float _bloodScreenDuration = 0.15f;

    [Header("Damage Popup")]
    [SerializeField] private DamagePopup _baseDamagePopupPrefab;
    [SerializeField] private DamagePopup _critDamagePopupPrefab;

    private CanvasGroup _bloodScreenCanvasGroup;

    private DamagePopup _popupInstance;

    private Coroutine _bloodScreenCoroutine;

    private void Awake()
    {
        _bloodScreenCanvasGroup = _bloodScreenObject.GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        _bloodScreenCanvasGroup.alpha = 0f;
    }

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

    public void ShowBloodScreen()
    {
        if (_bloodScreenCoroutine != null)
        {
            StopCoroutine(_bloodScreenCoroutine);
        }

        _bloodScreenCoroutine = StartCoroutine(BloodScreenRoutine());
    }

    private IEnumerator BloodScreenRoutine()
    {
        _bloodScreenObject.SetActive(true);
        _bloodScreenCanvasGroup.alpha = 1f;

        float elapsedTime = 0;
        float alpha = _bloodScreenCanvasGroup.alpha;

        while (elapsedTime <= _bloodScreenDuration)
        {
            elapsedTime += Time.deltaTime;

            alpha = Mathf.Lerp(1, 0, elapsedTime / _bloodScreenDuration);
            _bloodScreenCanvasGroup.alpha = alpha;

            yield return null;
        }

        _bloodScreenCanvasGroup.alpha = 0f;
        _bloodScreenObject.SetActive(false);
    }
}
