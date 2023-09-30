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
    [SerializeField] Vector2 cardDimensions; // Dimens�es da carta (largura e altura)

    [Header("Card Reference")]
    public CardScriptable card; // Refer�ncia ao script�vel que cont�m os dados da carta

    [Header("Card Infos References")]
    [SerializeField] TMP_Text cardName; // Refer�ncia ao componente de texto para o nome da carta
    [SerializeField] TMP_Text cardCost; // Refer�ncia ao componente de texto para o custo da carta
    [SerializeField] Image cardImage; // Refer�ncia � imagem que representa a carta
    [SerializeField] TMP_Text cardDamage; // Refer�ncia ao componente de texto para o dano da carta
    [SerializeField] TMP_Text cardWalk; // Refer�ncia ao componente de texto para a dist�ncia de movimento da carta
    [SerializeField] TMP_Text cardHealth; // Refer�ncia ao componente de texto para a sa�de (vida) da carta

    private RectTransform cardRect; // Refer�ncia ao ret�ngulo da carta

    public void SetupCardUI() {
        // Define os valores dos componentes de UI com base nos dados da carta
        cardName.text = card.CardName;
        cardCost.text = card.CardCost.ToString();
        cardImage.sprite = card.CardIcon;
        cardDamage.text = card.CardDamage.ToString();
        cardWalk.text = card.CardWalkDistance.ToString();
        cardHealth.text = card.CardHealth.ToString();

        // Obt�m a refer�ncia ao ret�ngulo da carta e armazena suas dimens�es
        cardRect = GetComponent<RectTransform>();
        cardDimensions = new Vector2(cardRect.rect.width, cardRect.rect.height);
    }

    public void OnPointerUp(PointerEventData eventData) {
        // L�gica a ser executada quando o ponteiro � liberado (n�o implementado neste c�digo)
    }

    public void OnPointerDown(PointerEventData eventData) {
        // Quando a carta � pressionada, chama o m�todo para selecionar a carta no controlador do jogador
        GameController.instance.PlayerController.SelectCard(card, this);
    }

    public void SelectCard() {
        // Aumenta o tamanho da carta quando � selecionada
        cardRect.sizeDelta = cardDimensions * 1.3f;
    }

    public void UnselectCard() {
        // Restaura o tamanho original da carta quando � desselecionada
        cardRect.sizeDelta = cardDimensions;
    }
}
