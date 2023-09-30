using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : PlayerBase
{
    [SerializeField] LayerMask gridLayer; // Camada utilizada para detectar o grid no mundo 3D
    [SerializeField] CardUIBase actualCardUI; // Representa a interface de usuário da carta atual

    // Propriedade para definir a interface de usuário da carta atual e lidar com seleções
    public CardUIBase ActualCardUI {
        set {
            // Desseleciona a carta anterior, se houver uma
            if (actualCardUI != null)
                actualCardUI.UnselectCard();
            actualCardUI = value;
            // Seleciona a nova carta, se houver uma
            if (value != null)
                actualCardUI.SelectCard();
        }
    }

    // Método chamado a cada quadro (frame)
    public void Update() {
        GetCells(); // Chama o método para detectar as células do grid
    }

    // Método para detectar as células do grid com base na posição do mouse
    void GetCells() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Cria um raio a partir da posição do mouse
        RaycastHit hit;

        // Detecta colisões com o grid
        if (Physics.Raycast(ray, out hit, 100, gridLayer)) {
            CellGrid cellGrid = hit.collider.GetComponent<CellGrid>();
            if (cellGrid == null)
                return;

            if (CurrentCell != null) CurrentCell.OnUnhover(); // Desativa o destaque da célula atual, se houver uma
            CurrentCell = cellGrid; // Define a nova célula como a célula atual
            CurrentCell.OnHover(); // Ativa o destaque na nova célula
            if (Input.GetMouseButton(0)) CurrentCell.OnClick(); // Se o botão esquerdo do mouse for clicado, chama a função de clique na célula

            return;
        }
    }

    // Método para selecionar uma carta
    public override void SelectCard(CardScriptable _card, CardUIBase cardUI = null) {
        if (CurrentCard != null && CurrentCard == _card) {
            CurrentCard = null;
            ActualCardUI = null;
            return;
        }
        CurrentCard = _card;
        ActualCardUI = cardUI;
    }

    // Método para configurar o jogador no início do jogo
    public override void SetupPlayer() {
        base.SetupPlayer(); // Chama o método da classe base para configurar a mana inicial
        DrawHandCards(); // Chama o método para distribuir cartas na mão do jogador
    }

    // Método para distribuir cartas na mão do jogador
    public void DrawHandCards() {
        foreach (CardScriptable card in ActualDeck) {
            GameController.instance.UIController.GetCardToHand(card); // Obtém uma carta para a mão do jogador
        }
    }
}
