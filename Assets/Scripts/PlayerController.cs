using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : PlayerBase
{
    [SerializeField] LayerMask gridLayer; // Camada utilizada para detectar o grid no mundo 3D
    [SerializeField] CardUIBase actualCardUI; // Representa a interface de usu�rio da carta atual

    // Propriedade para definir a interface de usu�rio da carta atual e lidar com sele��es
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

    // M�todo chamado a cada quadro (frame)
    public void Update() {
        GetCells(); // Chama o m�todo para detectar as c�lulas do grid
    }

    // M�todo para detectar as c�lulas do grid com base na posi��o do mouse
    void GetCells() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Cria um raio a partir da posi��o do mouse
        RaycastHit hit;

        // Detecta colis�es com o grid
        if (Physics.Raycast(ray, out hit, 100, gridLayer)) {
            CellGrid cellGrid = hit.collider.GetComponent<CellGrid>();
            if (cellGrid == null)
                return;

            if (CurrentCell != null) CurrentCell.OnUnhover(); // Desativa o destaque da c�lula atual, se houver uma
            CurrentCell = cellGrid; // Define a nova c�lula como a c�lula atual
            CurrentCell.OnHover(); // Ativa o destaque na nova c�lula
            if (Input.GetMouseButton(0)) CurrentCell.OnClick(); // Se o bot�o esquerdo do mouse for clicado, chama a fun��o de clique na c�lula

            return;
        }
    }

    // M�todo para selecionar uma carta
    public override void SelectCard(CardScriptable _card, CardUIBase cardUI = null) {
        if (CurrentCard != null && CurrentCard == _card) {
            CurrentCard = null;
            ActualCardUI = null;
            return;
        }
        CurrentCard = _card;
        ActualCardUI = cardUI;
    }

    // M�todo para configurar o jogador no in�cio do jogo
    public override void SetupPlayer() {
        base.SetupPlayer(); // Chama o m�todo da classe base para configurar a mana inicial
        DrawHandCards(); // Chama o m�todo para distribuir cartas na m�o do jogador
    }

    // M�todo para distribuir cartas na m�o do jogador
    public void DrawHandCards() {
        foreach (CardScriptable card in ActualDeck) {
            GameController.instance.UIController.GetCardToHand(card); // Obt�m uma carta para a m�o do jogador
        }
    }
}
