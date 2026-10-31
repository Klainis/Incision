using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashStamina : MonoBehaviour
{
    private PlayerMovementData _playerMovementData;

    private int _dashMaxCharges = 1;
    private float _dashCoolDown = 0;
    private float _dashDelay = 0;

    private float _stamina;
    private float _delayTimer;

    public int FullCharges => Mathf.FloorToInt(_stamina);
    public float Stamina => _stamina;
    public int MaxCharges => _dashMaxCharges;
    public bool CanDash => _stamina >= 1f;

    public event Action<float, int> Changed;

    public void Initialize(PlayerMovementData moveData)
    {
        _playerMovementData = moveData;

        _dashMaxCharges = _playerMovementData.DashMaxCharges;
        _dashDelay = _playerMovementData.DashDelay;
        _dashCoolDown = _playerMovementData.DashCoolDown;

        _stamina = _dashMaxCharges;
    }

    private void Update()
    {
        if (_stamina >= _dashMaxCharges)
            return;

        if (_delayTimer > 0f)
        {
            _delayTimer -= Time.deltaTime;
            return;
        }

        _stamina = Mathf.Min(_dashMaxCharges, _stamina + Time.deltaTime / _dashCoolDown);
        Changed?.Invoke(_stamina, _dashMaxCharges);
    }

    public bool TryConsumeStamina()
    {
        if (!CanDash)
            return false;

        _stamina -= 1f;
        _delayTimer = _dashDelay;
        Changed?.Invoke(_stamina, _dashMaxCharges);
        return true;
    }
}
