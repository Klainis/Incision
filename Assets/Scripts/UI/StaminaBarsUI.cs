using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarsUI : MonoBehaviour
{
    [SerializeField] private DashStamina _dashStamina;
    [SerializeField] private Image[] _bars;

    private void OnEnable() => _dashStamina.Changed += Refresh;
    private void OnDisable() => _dashStamina.Changed -= Refresh;

    private void Refresh(float currentStamina, int maxCharges)
    {
        for (int i = 0; i < _bars.Length; i++)
            _bars[i].fillAmount = Mathf.Clamp01(currentStamina - i);
    }
}
