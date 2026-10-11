using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationSpeed : MonoBehaviour
{
    [SerializeField] private CardHand _hand;
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationClip _throwClip;
    [SerializeField] private string _speedParameter = "AttackSpeedMultiplier";

    private int _speedHash;

    private void Awake()
    {
        _speedHash = Animator.StringToHash(_speedParameter);
    }

    private void OnEnable()
    {
        _hand.ActiveChanged += OnChanged;
        _hand.HandChanged += Apply;
    }

    private void OnDisable()
    {
        _hand.ActiveChanged -= OnChanged;
        _hand.HandChanged -= Apply;
    }

    private void Start() => Apply();

    private void OnChanged(int _) => Apply();

    private void Apply()
    {
        CardInstance card = _hand.ActiveCard;
        if (card == null) return;

        _animator.SetFloat(_speedHash, /*_throwClip.length * */card.Data.AttackSpeed);

        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log($"state speedMultiplier = {info.speedMultiplier}");
    }
}
