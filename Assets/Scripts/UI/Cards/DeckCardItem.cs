using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckCardItem : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _orderText;

    public void Set(CardInstance card, int order)
    {
        _icon.sprite = card.Data.CardIcon;
        _nameText.text = card.Data.CardName;
        _orderText.text = order.ToString();
    }
}
