using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarsUI : MonoBehaviour
{
    [SerializeField] private Image[] _bars;

    private DashStamina _dashStamina;

    public void Initialize(DashStamina stamina)
    {
        _dashStamina = stamina;
        _dashStamina.Changed += Refresh;
    }

    private void OnDisable()
    {
        _dashStamina.Changed -= Refresh;
    }

    private void Refresh(float currentStamina, int maxCharges)
    {
        for (int i = 0; i < _bars.Length; i++)
        {
            Debug.Log("REFRESHING STAMINA BARS");
            _bars[i].fillAmount = Mathf.Clamp01(currentStamina - i);
        }
    }
}
