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
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private TextMeshProUGUI _cardName;
    [SerializeField] private GameObject _abilityActiveIcon;
    [SerializeField] private GameObject _abilityInactiveIcon;
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
        _cardName.text = card.Data.CardName;
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

        _abilityActiveIcon.SetActive(!onCooldown);
        _abilityInactiveIcon.SetActive(onCooldown);

        if (onCooldown)
            _abilityFill.fillAmount = 1f - Mathf.Clamp01(remaining / _card.Data.AbilityCoolDown);
        else _abilityFill.fillAmount = 1f;
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
