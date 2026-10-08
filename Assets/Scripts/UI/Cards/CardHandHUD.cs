using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandHUD : MonoBehaviour
{
    [Header("Hand")]
    [SerializeField] private CardHand _hand;
    [SerializeField] private CardSlotView[] _slots;

    [Header("NextCard")]
    [SerializeField] private GameObject _nextRoot;
    [SerializeField] private Image _nextIcon;

    private void OnEnable()
    {
        _hand.HandChanged += OnHandChanged;
        _hand.ActiveChanged += OnActiveChanged;
        _hand.AmmoChanged += OnAmmoChanged;
    }

    private void OnDisable()
    {
        _hand.HandChanged -= OnHandChanged;
        _hand.ActiveChanged -= OnActiveChanged;
        _hand.AmmoChanged -= OnAmmoChanged;
    }

    private void Start()
    {
        RefreshAll(snap: true);
    }

    private void OnHandChanged() => RefreshAll(snap: true);

    private void OnActiveChanged(int index) => RefreshActive();

    private void OnAmmoChanged(CardInstance card)
    {
        int slot = IndexOf(card);
        if (slot >= 0)
            _slots[slot].SetAmmo(card);
        // Карты нет в руке (ушла в колоду): HUD её не показывает
    }

    private void RefreshAll(bool snap)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            CardInstance card = i < _hand.Hand.Count ? _hand.Hand[i] : null;
            _slots[i].SetCard(card, snap);
        }

        CardInstance next = _hand.NextCardInDeck;

        _nextRoot.SetActive(next != null);
        if (next != null) _nextIcon.sprite = next.Data.CardIcon;

        RefreshActive();
    }

    private void RefreshActive()
    {
        for (int i = 0; i < _slots.Length; i++)
            _slots[i].SetActiveSlot(i == _hand.ActiveIndex);
    }

    private int IndexOf(CardInstance card)
    {
        for (int i = 0; i < _hand.Hand.Count && i < _slots.Length; i++)
        {
            if (_hand.Hand[i] == card)
                return i;
        }
        return -1;
    }
}
