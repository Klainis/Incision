using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardSlotView : MonoBehaviour
{
    [SerializeField] private Image _cardIcon;
/*    [SerializeField] private Image _ammoFill; */         // Image Type = Filled
    [SerializeField] private TMP_Text _ammoText;
    //[SerializeField] private TMP_Text _nameText;
    [SerializeField] private GameObject _activeFrame;
    [SerializeField] private CanvasGroup _group;

    [Header("Animation")]
    [SerializeField] private float _inactiveAlpha = 0.6f;
    [SerializeField] private float _inactiveScale = 0.9f;
    [SerializeField] private float _fillSpeed = 3f;    // доля полоски в секунду
    [SerializeField] private float _lerpSpeed = 12f;

    private float _targetFill;
    private float _targetAlpha = 1f;
    private float _targetScale = 1f;

    public void SetCard(CardInstance card, bool snap = true)
    {
        bool hasCard = card != null;
        //gameObject.SetActive(hasCard);
        SetActiveSlot(hasCard);
        if (!hasCard) return;

        _cardIcon.sprite = card.Data.CardIcon;
        //if (_nameText != null) _nameText.text = card.Data.DisplayName;
        //if (_ammoFill != null) _ammoFill.color = card.Data.AccentColor;

        SetAmmo(card);

        //if (snap && _ammoFill != null)
        //    _ammoFill.fillAmount = _targetFill;
    }

    public void SetAmmo(CardInstance card)
    {
        if (card == null) return;

        //_targetFill = (float)card.Ammo / card.Data.MaxAmmo;

        if (_ammoText != null)
            _ammoText.text = $"{card.Ammo}/{card.Data.MaxAmmo}";
    }

    public void SetActiveSlot(bool isActive)
    {
        //if (_activeFrame != null) _activeFrame.SetActive(isActive);

        _targetAlpha = isActive ? 1f : _inactiveAlpha;
        _targetScale = isActive ? 1f : _inactiveScale;
        if (isActive) gameObject.transform.SetAsLastSibling();
    }

    private void Update()
    {
        float dt = Time.unscaledDeltaTime;

        //if (_ammoFill != null)
        //    _ammoFill.fillAmount = Mathf.MoveTowards(_ammoFill.fillAmount, _targetFill, _fillSpeed * dt);

        if (_group != null)
            _group.alpha = Mathf.Lerp(_group.alpha, _targetAlpha, _lerpSpeed * dt);

        float s = Mathf.Lerp(transform.localScale.x, _targetScale, _lerpSpeed * dt);
        transform.localScale = new Vector3(s, s, 1f);
    }
}
