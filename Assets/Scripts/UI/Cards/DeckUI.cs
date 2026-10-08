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
    [SerializeField] private Transform _listRoot;
    [SerializeField] private DeckCardItem _itemPrefab;
    [SerializeField] private InputActionReference _toggleAction; // Button, например Tab

    private readonly List<DeckCardItem> _items = new List<DeckCardItem>();

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

        foreach (CardInstance card in _hand.Deck)
        {
            if (order >= _items.Count)
                _items.Add(Instantiate(_itemPrefab, _listRoot));

            _items[order].gameObject.SetActive(true);
            _items[order].Set(card, order + 1);
            order++;
        }

        // Ћишние строки пр€чем, а не удал€ем: так нет мусора дл€ GC
        for (int i = order; i < _items.Count; i++)
            _items[i].gameObject.SetActive(false);
    }
}
