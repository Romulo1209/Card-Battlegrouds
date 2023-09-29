using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;

public class CardUIBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Card Parameters")]
    [SerializeField] Vector2 cardDimensions;

    [Header("Card Reference")]
    public CardScriptable card;

    [Header("Card Infos References")]
    [SerializeField] TMP_Text cardName;
    [SerializeField] TMP_Text cardCost;
    [SerializeField] Image cardImage;
    [SerializeField] TMP_Text cardDamage;
    [SerializeField] TMP_Text cardWalk;
    [SerializeField] TMP_Text cardHealth;

    private RectTransform cardRect;

    public void SetupCardUI() {
        cardName.text = card.CardName;
        cardCost.text = card.CardCost.ToString();
        cardImage.sprite = card.CardIcon;
        cardDamage.text = card.CardDamage.ToString();
        cardWalk.text = card.CardWalkDistance.ToString();
        cardHealth.text = card.CardHealth.ToString();

        cardRect = GetComponent<RectTransform>();
        cardDimensions = new Vector2(cardRect.rect.width, cardRect.rect.height);
    }

    public void OnPointerUp(PointerEventData eventData) {
        GameController.instance.PlayerController.SelectCard(card, this);
    }

    public void OnPointerDown(PointerEventData eventData) {
        
    }

    public void SelectCard() {
        cardRect.sizeDelta = cardDimensions * 1.3f;
    }

    public void UnselectCard() {
        cardRect.sizeDelta = cardDimensions;
    }
}
