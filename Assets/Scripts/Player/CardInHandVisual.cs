using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInHandVisual : MonoBehaviour
{
    [SerializeField] private CardHand _hand;
    [SerializeField] private Renderer _cardRenderer;   // меш карты в руке
    [SerializeField] private Animator _animator;       // необязательно
    [SerializeField] private string _swapTrigger = "SwapCard";
    [SerializeField] private float _swapDelay = 0f;    // момент смены лица в анимации смены

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

    // Игрок выбрал другой слот: анимация и смена лица с задержкой
    private void OnActiveChanged(int _)
    {
        if (_animator != null && !string.IsNullOrEmpty(_swapTrigger))
            _animator.SetTrigger(_swapTrigger);

        if (_routine != null) StopCoroutine(_routine);
        _routine = StartCoroutine(ApplyDelayed());
    }

    // Активная карта закончилась и заменилась: лицо меняется сразу
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
