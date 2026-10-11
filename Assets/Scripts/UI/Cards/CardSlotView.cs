using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardSlotView : MonoBehaviour
{
    [SerializeField] private Image _cardIcon;
    [SerializeField] private Image _abilityFill;
    [SerializeField] private TMP_Text _ammoText;
    [SerializeField] private GameObject _abilityFrame;
    [SerializeField] private CanvasGroup _group;

    [Header("Animation")]
    [SerializeField] private float _inactiveAlpha = 0.6f;
    [SerializeField] private float _inactiveScale = 0.9f;
    [SerializeField] private float _lerpSpeed = 12f;

    private CardInstance _card;
    private float _targetAlpha = 1f;
    private float _targetScale = 1f;

    public void SetCard(CardInstance card, bool snap = true)
    {
        _card = card;

        if (card == null) return;

        _cardIcon.sprite = card.Data.CardIcon;
        SetAmmo(card);
        UpdateAbilityFill();
    }

    public void SetAmmo(CardInstance card)
    {
        if (card == null || _ammoText == null) return;
        _ammoText.text = $"{card.Ammo}/{card.Data.MaxAmmo}";
    }

    public void SetActiveSlot(bool isActive)
    {
        _targetAlpha = isActive ? 1f : _inactiveAlpha;
        _targetScale = isActive ? 1f : _inactiveScale;
        if (isActive) transform.SetAsLastSibling();
    }

    private void UpdateAbilityFill()
    {
        if (_card == null || _abilityFill == null) return;

        float remaining = _card.NextAbilityTime - Time.time;
        bool onCooldown = remaining > 0f;

        if (_abilityFrame.activeSelf != onCooldown)
            _abilityFrame.SetActive(onCooldown);

        if (onCooldown)
            _abilityFill.fillAmount = Mathf.Clamp01(remaining / _card.Data.AbilityCoolDown);
    }

    private void Update()
    {
        UpdateAbilityFill();

        float dt = Time.unscaledDeltaTime;

        if (_group != null)
            _group.alpha = Mathf.Lerp(_group.alpha, _targetAlpha, _lerpSpeed * dt);

        float s = Mathf.Lerp(transform.localScale.x, _targetScale, _lerpSpeed * dt);
        transform.localScale = new Vector3(s, s, 1f);
    }
}
