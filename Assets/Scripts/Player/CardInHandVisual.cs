using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInHandVisual : MonoBehaviour
{
    [SerializeField] private CardHand _hand;
    [SerializeField] private Renderer _cardRenderer;
    [SerializeField] private PlayerAnimation _playerAnimation;
    [SerializeField] private float _swapDelay = 0f;

    private Coroutine _routine;

    private void OnEnable()
    {
        _hand.ActiveChanged += OnActiveChanged;
        _hand.HandChanged += OnHandChanged;
    }

    private void OnDisable()
    {
        _hand.ActiveChanged -= OnActiveChanged;
        _hand.HandChanged -= OnHandChanged;
    }

    private void Start()
    {
        Apply();
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
