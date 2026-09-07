using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private Camera _facingCamera;
    [SerializeField] private TextMeshProUGUI _popupText;
    [SerializeField] private float _timeToDestroy;
    [SerializeField] private float _offsetMinX = 0;
    [SerializeField] private float _offsetMaxX = 0;
    [SerializeField] private float _offsetMinY = 0;
    [SerializeField] private float _offsetMaxY = 0;

    private Vector3 _targetWorldPosition;

    private Coroutine _destroyCoroutine;

    private void Awake()
    {
        _facingCamera = Camera.main;
        if (_popupText == null) _popupText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _destroyCoroutine = StartCoroutine(DestroyTimer());
    }

    private void LateUpdate()
    {
        //if (_facingCamera)
        //{
        //    transform.LookAt(2 * transform.position - _facingCamera.transform.position);
        //}
        UpdateScreenPosition(_targetWorldPosition);
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(_timeToDestroy);
        Destroy(gameObject);
    }

    public void SetDamageText(Vector3 worldPosition, float damage)
    {
        _popupText.text = $"{damage}";

        Vector3 worldOffset = new Vector3(Random.Range(_offsetMinX, _offsetMaxX), Random.Range(_offsetMinY, _offsetMaxY), 0f);
        Vector3 initialWorldPosiotin = worldPosition + worldOffset;
        UpdateScreenPosition(initialWorldPosiotin);
    }

    public void UpdateScreenPosition(Vector3 worldPosition)
    {
        if (_facingCamera == null) _facingCamera = Camera.main;

        _targetWorldPosition = worldPosition;
        Vector3 screenPos = _facingCamera.WorldToScreenPoint(worldPosition);


        if (screenPos.z < 0)
        {
            _popupText.enabled = false;
            return;
        }

        _popupText.enabled = true;
        transform.position = screenPos;
    }
}
