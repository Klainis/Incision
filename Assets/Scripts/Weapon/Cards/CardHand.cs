using Incision;
using Incision.Arcana;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

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

    private PlayerAttack _playerAttack;
    private PlayerAnimation _playerAnimation;
    private AttackAnimationSpeed _attackAnimationSpeed;
    private ArcanaCombatModifiers _arcanaModifiers;

    private CardInstance[] _hand;
    private readonly Queue<CardInstance> _deck = new Queue<CardInstance>();
    private readonly HashSet<CardInstance> _spentThisCycle = new HashSet<CardInstance>();
    private int _cycleCardCount;
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
    public bool CanUseAbility => ActiveCard != null && !ActiveCard.IsEmpty && ActiveCard.AbilityRuntime != null && Time.time >= ActiveCard.NextAbilityTime;

    public event Action HandChanged;
    public event Action<int> ActiveChanged;
    public event Action<CardInstance> AmmoChanged;
    public event Action<CardInstance> AbilityUsed;

    [Inject]
    private void SetArcanaModifiers(ArcanaCombatModifiers arcanaModifiers) => _arcanaModifiers = arcanaModifiers;

    public void Initialize(PlayerAttack attack, PlayerAnimation animation, AttackAnimationSpeed attackAnimationSpeed)
    {
        _playerAttack = attack;
        _playerAnimation = animation;
        _attackAnimationSpeed = attackAnimationSpeed;

        _hand = new CardInstance[_handSize];
        _deck.Clear();
        _spentThisCycle.Clear();
        _cycleCardCount = _startingCards.Count;
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
            _attackAnimationSpeed?.ApplyAttackSpeed(GetAttackSpeed(ActiveCard.Data));
            _playerAnimation.ThrowCardTriggerAnimation();
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
        bool isLastCharge = card.Ammo == 1 && data.SpendAttackAmmo == 1;
        damage *= _arcanaModifiers?.GetDamageMultiplier(_spentThisCycle.Count, _cycleCardCount, isLastCharge) ?? 1f;
        card.NextShotTime = Time.time + 1f / GetAttackSpeed(data);

        return SpendAttackAmmo(card, data.SpendAttackAmmo);
    }

    internal float GetAttackSpeed(CardData data) =>
        data.AttackSpeed * (_arcanaModifiers?.GetAttackSpeedMultiplier(Time.time) ?? 1f);

    public bool TryConsumeAbility(out CardData data)
    {
        data = null;

        CardInstance card = ActiveCard;

        if (card == null ||
            card.IsEmpty ||
            card.AbilityRuntime == null ||
            Time.time < card.NextAbilityTime)
        {
            return false;
        }

        if (!SpendAbilityAmmo(card, card.Data.SpendAbilityAmmo))
            return false;

        data = card.Data;

        card.NextAbilityTime = Time.time + data.AbilityCoolDown;

        AbilityContext ctx = new AbilityContext(this, _playerAttack, card);
        card.AbilityRuntime.Activate(ctx);

        return true;
    }

    public bool SpendAttackAmmo(CardInstance card, int amount)
    {
        if (!card.TrySpendAmmo(amount))
            return false;

        AmmoChanged?.Invoke(card);

        if (card.IsEmpty)
            CycleOut(Array.IndexOf(_hand, card));

        return true;
    }

    public bool SpendAbilityAmmo(CardInstance card, int amount)
    {
        if (!card.TrySpenAbilityAmmo(amount))
            return false;

        AbilityUsed?.Invoke(card);
        AmmoChanged?.Invoke(card);

        if (card.IsEmpty)
            CycleOut(Array.IndexOf(_hand, card));

        return true;
    }

    private void CycleOut(int slot)
    {
        CardInstance spent = _hand[slot];

        _spentThisCycle.Add(spent);
        if (_spentThisCycle.Count >= _cycleCardCount)
            _spentThisCycle.Clear();

        spent.Reload();

        if (_deck.Count > 0)
        {
            _hand[slot] = _deck.Dequeue();
            _deck.Enqueue(spent);
        }

        HandChanged?.Invoke();
        AmmoChanged?.Invoke(_hand[slot]);
    }

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= _hand.Length || index == _activeIndex)
            return;

        if (_hand[index] == null)
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
