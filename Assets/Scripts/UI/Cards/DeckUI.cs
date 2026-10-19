using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DeckUI : MonoBehaviour
{
    [SerializeField] private CardHand _hand;

    [Header("Deck Panel")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private Transform _deckListRoot;
    [SerializeField] private Transform _handListRoot;
    [SerializeField] private DeckCardItem _itemPrefab;
    [SerializeField] private InputActionReference _toggleAction; // Button, потом переместить в главный ui класс

    private readonly List<DeckCardItem> _deckItems = new List<DeckCardItem>();
    private readonly List<DeckCardItem> _handItems = new List<DeckCardItem>();


    private void Awake()
    {
        if (_hand == null)
        {
            _hand = FindAnyObjectByType<CardHand>();
        }
    }

    private void OnEnable()
    {
        _hand.HandChanged += Refresh;

        if (_toggleAction != null)
        {
            _toggleAction.action.Enable();
            _toggleAction.action.performed += OnToggle;
        }
    }

    private void OnDisable()
    {
        _hand.HandChanged -= Refresh;

        if (_toggleAction != null)
            _toggleAction.action.performed -= OnToggle;
    }

    private void Start()
    {
        if (_panel != null) _panel.SetActive(false);
        Refresh();
    }

    private void OnToggle(InputAction.CallbackContext _)
    {
        if (_panel == null) return;

        _panel.SetActive(!_panel.activeSelf);
        if (_panel.activeSelf) RebuildList();
    }

    private void Refresh()
    {

        if (_panel != null && _panel.activeSelf)
            RebuildList();
    }

    private void RebuildList()
    {
        int order = 0;

        foreach (CardInstance card in _hand.Hand)
        {
            if (order >= _handItems.Count)
                _handItems.Add(Instantiate(_itemPrefab, _handListRoot));

            _handItems[order].gameObject.SetActive(true);
            _handItems[order].Set(card, order + 1);
            order++;
        }

        order = 0;

        foreach (CardInstance card in _hand.Deck)
        {
            if (order >= _deckItems.Count)
                _deckItems.Add(Instantiate(_itemPrefab, _deckListRoot));

            _deckItems[order].gameObject.SetActive(true);
            _deckItems[order].Set(card, order + 1);
            order++;
        }

        for (int i = order; i < _deckItems.Count; i++)
            _deckItems[i].gameObject.SetActive(false);
    }
}
