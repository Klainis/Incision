using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHand : MonoBehaviour
{
    [SerializeField] private int _handSize = 2;
    [SerializeField] private List<CardData> _startingCards = new List<CardData>();

    private CardInstance[] _hand;
    private readonly Queue<CardInstance> _deck = new Queue<CardInstance>();
    private int _activeIndex;

    public int ActiveIndex => _activeIndex;
    public CardInstance ActiveCard => _hand[_activeIndex];
    public IReadOnlyList<CardInstance> Hand => _hand;
    public CardInstance NextCardInDeck => _deck.Count > 0 ? _deck.Peek() : null;

    public bool CanShoot => ActiveCard != null && !ActiveCard.IsEmpty && Time.time >= ActiveCard.NextShotTime;

    public event Action HandChanged;                // карта в слоте заменилась
    public event Action<int> ActiveChanged;         // сменилс€ активный слот
    public event Action<CardInstance> AmmoChanged;  // изменилс€ боезапас

    private void Awake()
    {
        _hand = new CardInstance[_handSize];
        int slot = 0;

        foreach (CardData data in _startingCards)
        {
            var card = new CardInstance(data);

            if (slot < _handSize) _hand[slot++] = card;
            else _deck.Enqueue(card);
        }
    }

    // ¬ызываетс€ в момент реального броска (событие анимации).
    // ¬озвращает данные карты и рассчитанный урон, тратит 1 боезапас.
    public bool TryConsumeShot(out CardData data, out float damage, out bool isCrit)
    {
        data = null;
        damage = 0f;
        isCrit = false;

        CardInstance card = ActiveCard;

        if (card == null || card.IsEmpty || Time.time < card.NextShotTime)
            return false;

        damage = card.RollDamage(out isCrit);
        data = card.Data;
        card.NextShotTime = Time.time + 1f / card.Data.AttackSpeed;

        // ѕоследним: если патроны кончатс€, карта уйдЄт в колоду, а данные уже сохранены
        return SpendAmmo(card, 1);
    }

    // ќбщий метод траты боезапаса, пригодитс€ и дл€ активного умени€ (клавиша E)
    public bool SpendAmmo(CardInstance card, int amount)
    {
        if (!card.TrySpendAmmo(amount))
            return false;

        AmmoChanged?.Invoke(card);

        if (card.IsEmpty)
            CycleOut(Array.IndexOf(_hand, card));

        return true;
    }

    private void CycleOut(int slot)
    {
        CardInstance spent = _hand[slot];

        spent.Reload();

        if (_deck.Count > 0)
        {
            _hand[slot] = _deck.Dequeue(); // на еЄ место встаЄт следующа€
            _deck.Enqueue(spent);          // пуста€ уходит в конец колоды
        }
        // иначе других карт нет: карта просто перезар€дилась на месте

        HandChanged?.Invoke();
        AmmoChanged?.Invoke(_hand[slot]);
    }

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= _hand.Length || index == _activeIndex)
            return;

        if (_hand[index] == null) // пустой слот
            return;

        _activeIndex = index;
        ActiveChanged?.Invoke(_activeIndex);
    }

    public void SelectRelative(int direction)
    {
        int next = (_activeIndex + direction + _hand.Length) % _hand.Length;
        SelectSlot(next);
    }
}
