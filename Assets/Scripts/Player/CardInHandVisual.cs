using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInHandVisual : MonoBehaviour
{
    [SerializeField] private Renderer _cardRenderer;
    [SerializeField] private float _swapDelay = 0f;

    private CardHand _hand;
    private PlayerAnimation _playerAnimation;

    private Coroutine _routine;

    public void Initialize(CardHand hand, PlayerAnimation animation)
    {
        _hand = hand;
        _playerAnimation = animation;

        _hand.ActiveChanged += OnActiveChanged;
        _hand.HandChanged += OnHandChanged;

        Apply();
    }

    private void OnDisable()
    {
        _hand.ActiveChanged -= OnActiveChanged;
        _hand.HandChanged -= OnHandChanged;
    }

    private void OnActiveChanged(int _)
    {
        _playerAnimation.SwapCardAnimation();

        if (_routine != null) StopCoroutine(_routine);
        _routine = StartCoroutine(ApplyDelayed());
    }

    private void OnHandChanged()
    {
        if (_routine != null)
        {
            StopCoroutine(_routine);
            _routine = null;
        }

        Apply();
    }

    private IEnumerator ApplyDelayed()
    {
        if (_swapDelay > 0f)
            yield return new WaitForSeconds(_swapDelay);

        Apply();
        _routine = null;
    }

    private void Apply()
    {
        CardInstance card = _hand.ActiveCard;
        if (card == null || card.Data.CardHandMaterial == null) return;

        _cardRenderer.sharedMaterial = card.Data.CardHandMaterial;
    }
}
