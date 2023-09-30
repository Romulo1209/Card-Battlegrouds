using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Main UI References")]
    [SerializeField] TMP_Text playerTurnText; // Refer�ncia ao texto que exibe de quem � o turno
    [SerializeField] TMP_Text manaText; // Refer�ncia ao texto que exibe a quantidade de mana
    [SerializeField] TMP_Text roundText; // Refer�ncia ao texto que exibe o n�mero do round
    [SerializeField] TMP_Text troopInfos; // Refer�ncia ao texto que exibe informa��es sobre a tropa
    [SerializeField] Transform handTransform; // Refer�ncia ao painel onde as cartas da m�o s�o exibidas

    [Header("Game Over References")]
    [SerializeField] TMP_Text headerText; // Refer�ncia ao texto de cabe�alho na tela de Game Over
    [SerializeField] TMP_Text highScore; // Refer�ncia ao texto que exibe a pontua��o mais alta
    [SerializeField] TMP_Text scoreText; // Refer�ncia ao texto que exibe a pontua��o atual

    [SerializeField] GameObject cardBase; // Prefab da carta que � usada para criar cartas na m�o

    [SerializeField] List<WindowBase> windows; // Lista de janelas no jogo

    List<GameObject> handObjects = new List<GameObject>(); // Lista de objetos de cartas na m�o do jogador

    // Fun��o para atualizar o HUD (Heads-Up Display) com informa��es como turno, mana e round
    public void UpdateHUD(int actualMana, int actualRound, Players playerTurn) {
        playerTurnText.text = playerTurn == Players.Player ? "Player 1 Turn" : "Player 2 Turn";
        manaText.text = $"Mana \n{actualMana}";
        roundText.text = $"Round \n{actualRound}";
    }

    // Fun��o para configurar a tela de Game Over com base no vencedor e na pontua��o
    public void SetGameOverWindow(Players winner, int score) {
        switch (winner) {
            case Players.Player:
                headerText.text = "You Win";
                headerText.color = Color.green;
                break;
            case Players.Bot:
                headerText.text = "Game Over";
                headerText.color = Color.red;
                break;
        }
        highScore.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();
        scoreText.text = "Your Score: " + score.ToString();
    }

    // Fun��o para exibir informa��es sobre uma tropa (carta)
    public void SetTroopInfos(CardScriptable card) {
        if (card == null) {
            troopInfos.text = null;
            return;
        }
        troopInfos.text = $"Troop: {card.CardName} \nCard Damage: {card.CardDamage} \nCard Walk Distance: {card.CardWalkDistance} \nCard Attack Distance: {card.CardAttackDistance}";
    }

    // Fun��o para redefinir a interface das cartas na m�o do jogador
    public void ResetHandCardsUI() {
        for (int i = 0; i < handObjects.Count; i++)
            Destroy(handObjects[i]);
        handObjects.Clear();
    }

    // Fun��o para adicionar uma carta � m�o do jogador na interface
    public void GetCardToHand(CardScriptable card) {
        GameObject _card = Instantiate(cardBase, handTransform);
        _card.GetComponent<CardUIBase>().card = card;
        _card.GetComponent<CardUIBase>().SetupCardUI();
        handObjects.Add(_card);
    }
}
