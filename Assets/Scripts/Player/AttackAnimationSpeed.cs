using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationSpeed : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationClip _throwClip;
    [SerializeField] private string _speedParameter = "AttackSpeedMultiplier";

    private CardHand _hand;

    private int _speedHash;

    public void Initialize(CardHand hand)
    {
        _hand = hand;

        _speedHash = Animator.StringToHash(_speedParameter);

        _hand.ActiveChanged += OnChanged;
        _hand.HandChanged += Apply;

        Apply();
    }

    private void OnDisable()
    {
        _hand.ActiveChanged -= OnChanged;
        _hand.HandChanged -= Apply;
    }

    private void OnChanged(int _) => Apply();

    private void Apply()
    {
        CardInstance card = _hand.ActiveCard;
        if (card == null) return;

        _animator.SetFloat(_speedHash, card.Data.AttackSpeed / _throwClip.length);

        //AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
        //Debug.Log($"state speedMultiplier = {info.speedMultiplier}");
    }
}
