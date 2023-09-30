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
    [SerializeField] Vector2 cardDimensions; // Dimensões da carta (largura e altura)

    [Header("Card Reference")]
    public CardScriptable card; // Referência ao scriptável que contém os dados da carta

    [Header("Card Infos References")]
    [SerializeField] TMP_Text cardName; // Referência ao componente de texto para o nome da carta
    [SerializeField] TMP_Text cardCost; // Referência ao componente de texto para o custo da carta
    [SerializeField] Image cardImage; // Referência à imagem que representa a carta
    [SerializeField] TMP_Text cardDamage; // Referência ao componente de texto para o dano da carta
    [SerializeField] TMP_Text cardWalk; // Referência ao componente de texto para a distância de movimento da carta
    [SerializeField] TMP_Text cardHealth; // Referência ao componente de texto para a saúde (vida) da carta

    private RectTransform cardRect; // Referência ao retângulo da carta

    public void SetupCardUI() {
        // Define os valores dos componentes de UI com base nos dados da carta
        cardName.text = card.CardName;
        cardCost.text = card.CardCost.ToString();
        cardImage.sprite = card.CardIcon;
        cardDamage.text = card.CardDamage.ToString();
        cardWalk.text = card.CardWalkDistance.ToString();
        cardHealth.text = card.CardHealth.ToString();

        // Obtém a referência ao retângulo da carta e armazena suas dimensões
        cardRect = GetComponent<RectTransform>();
        cardDimensions = new Vector2(cardRect.rect.width, cardRect.rect.height);
    }

    public void OnPointerUp(PointerEventData eventData) {
        // Lógica a ser executada quando o ponteiro é liberado (não implementado neste código)
    }

    public void OnPointerDown(PointerEventData eventData) {
        // Quando a carta é pressionada, chama o método para selecionar a carta no controlador do jogador
        GameController.instance.PlayerController.SelectCard(card, this);
    }

    public void SelectCard() {
        // Aumenta o tamanho da carta quando é selecionada
        cardRect.sizeDelta = cardDimensions * 1.3f;
    }

    public void UnselectCard() {
        // Restaura o tamanho original da carta quando é desselecionada
        cardRect.sizeDelta = cardDimensions;
    }
}
