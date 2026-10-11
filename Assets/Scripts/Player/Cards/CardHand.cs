using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerCardInput
{
    public bool Shoot;
    public bool Ability;
    public Vector2 SwitchScroll;
    public bool FirstCardSlot;
    public bool SecondCardSlot;
}

public class CardHand : MonoBehaviour
{
    [SerializeField] private int _handSize = 2;
    [SerializeField] private List<CardData> _startingCards = new List<CardData>();

    private CardInstance[] _hand;
    private readonly Queue<CardInstance> _deck = new Queue<CardInstance>();
    private int _activeIndex;

    private bool _requestedShoot;
    private bool _requestedAbility;
    private bool _requestedFirstCardSlot;
    private bool _requestedSecondCardSlot;

    private float _requestedSwitchScroll;

    public int ActiveIndex => _activeIndex;
    public CardInstance ActiveCard => _hand[_activeIndex];
    public IReadOnlyList<CardInstance> Hand => _hand;
    public CardInstance NextCardInDeck => _deck.Count > 0 ? _deck.Peek() : null;
    public int DeckCount => _deck.Count;
    public IEnumerable<CardInstance> Deck => _deck;

    public bool CanShoot => ActiveCard != null && !ActiveCard.IsEmpty && Time.time >= ActiveCard.NextShotTime;
    public bool CanUseAbility => ActiveCard != null && !ActiveCard.IsEmpty && Time.time >= ActiveCard.NextAbilityTime;

    public event Action HandChanged;                // карта в слоте заменилась
    public event Action<int> ActiveChanged;         // сменился активный слот
    public event Action<CardInstance> AmmoChanged;  // изменился боезапас

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

    private void Update()
    {
        HandleSwitch();
        HandleShoot();
        HandleAbility();
    }

    public void UpdateInput(PlayerCardInput input)
    {
        _requestedShoot = input.Shoot;
        _requestedSwitchScroll = input.SwitchScroll.y;
        _requestedAbility = input.Ability;
        _requestedFirstCardSlot = input.FirstCardSlot;
        _requestedSecondCardSlot = input.SecondCardSlot;
    }

    private void HandleSwitch()
    {
        if (_requestedSwitchScroll > 0.1f)
        {
            SelectRelative(-1);
        }
        else if (_requestedSwitchScroll < -0.1f)
        {
            SelectRelative(1);
        }

        if (_requestedFirstCardSlot)
        {
            SelectSlot(0);
        }
        else if (_requestedSecondCardSlot)
        {
            SelectSlot(1);
        }
    }

    private void HandleShoot()
    {
        if (_requestedShoot && CanShoot)
        {
            PlayerAnimation.Instance.ThrowCardTriggerAnimation();
        }
    }

    private void HandleAbility()
    {
        if (_requestedAbility && CanUseAbility)
        {
            TryConsumeAbility(out CardData data);
        }
    }

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
        card.NextShotTime = Time.time + 1f / data.AttackSpeed;

        return SpendAmmo(card, data.SpendAttackAmmo);
    }

    public bool TryConsumeAbility(out CardData data)
    {
        data = null;

        CardInstance card = ActiveCard;

        if (card == null || card.IsEmpty || Time.time < card.NextAbilityTime)
            return false;

        data = card.Data;
        card.NextAbilityTime = Time.time + data.AbilityCoolDown;

        return SpendAmmo(card, data.SpendAbilityAmmo);
    }

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
            _hand[slot] = _deck.Dequeue(); // на её место встаёт следующая
            _deck.Enqueue(spent);          // пустая уходит в конец колоды
        }

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
