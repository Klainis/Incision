using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandHUD : MonoBehaviour
{
    [SerializeField] private CardHand _hand;
    [SerializeField] private CardSlotView[] _slots;

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

    // Start, а не OnEnable: к этому моменту CardHand.Awake уже создал руку
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
        //  арты нет в руке (ушла в колоду): HUD еЄ не показывает
    }

    private void RefreshAll(bool snap)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            CardInstance card = i < _hand.Hand.Count ? _hand.Hand[i] : null;
            _slots[i].SetCard(card, snap);
        }

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
