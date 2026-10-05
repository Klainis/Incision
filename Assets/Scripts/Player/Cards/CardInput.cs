using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardInput : MonoBehaviour
{
    [SerializeField] private CardHand _hand;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference _shoot;        // Button
    [SerializeField] private InputActionReference _switchScroll; // Value, Vector2 (Mouse/Scroll)
    [SerializeField] private InputActionReference[] _slotActions; // [0] = клавиша 1, [1] = клавиша 2

    private bool _isAttacking = false;

    private void OnEnable()
    {
        _shoot.action.Enable();
        _switchScroll.action.Enable();

        foreach (var slot in _slotActions)
            slot.action.Enable();
    }

    private void Update()
    {
        // Пока идёт бросок, карту менять нельзя и новый бросок не запускается
        if (_isAttacking)
            return;

        HandleSwitch();
        HandleShoot();
    }

    private void HandleSwitch()
    {
        float scroll = _switchScroll.action.ReadValue<Vector2>().y;

        if (scroll > 0.1f) _hand.SelectRelative(-1);
        else if (scroll < -0.1f) _hand.SelectRelative(1);

        for (int i = 0; i < _slotActions.Length; i++)
        {
            if (_slotActions[i].action.WasPressedThisFrame())
                _hand.SelectSlot(i);
        }
    }

    private void HandleShoot()
    {
        // IsPressed уже включает кадр нажатия, отдельная проверка WasPressedThisFrame не нужна
        if (_shoot.action.IsPressed() && _hand.CanShoot)
        {
            PlayerAnimation.Instance.ThrowCardTrigger();
        }
    }
}
