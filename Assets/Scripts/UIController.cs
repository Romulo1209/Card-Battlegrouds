using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Main UI References")]
    [SerializeField] TMP_Text playerTurnText; // Referência ao texto que exibe de quem é o turno
    [SerializeField] TMP_Text manaText; // Referência ao texto que exibe a quantidade de mana
    [SerializeField] TMP_Text roundText; // Referência ao texto que exibe o número do round
    [SerializeField] TMP_Text troopInfos; // Referência ao texto que exibe informações sobre a tropa
    [SerializeField] Transform handTransform; // Referência ao painel onde as cartas da mão são exibidas

    [Header("Game Over References")]
    [SerializeField] TMP_Text headerText; // Referência ao texto de cabeçalho na tela de Game Over
    [SerializeField] TMP_Text highScore; // Referência ao texto que exibe a pontuação mais alta
    [SerializeField] TMP_Text scoreText; // Referência ao texto que exibe a pontuação atual

    [SerializeField] GameObject cardBase; // Prefab da carta que é usada para criar cartas na mão

    [SerializeField] List<WindowBase> windows; // Lista de janelas no jogo

    List<GameObject> handObjects = new List<GameObject>(); // Lista de objetos de cartas na mão do jogador

    // Função para atualizar o HUD (Heads-Up Display) com informações como turno, mana e round
    public void UpdateHUD(int actualMana, int actualRound, Players playerTurn) {
        playerTurnText.text = playerTurn == Players.Player ? "Player 1 Turn" : "Player 2 Turn";
        manaText.text = $"Mana \n{actualMana}";
        roundText.text = $"Round \n{actualRound}";
    }

    // Função para configurar a tela de Game Over com base no vencedor e na pontuação
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

    // Função para exibir informações sobre uma tropa (carta)
    public void SetTroopInfos(CardScriptable card) {
        if (card == null) {
            troopInfos.text = null;
            return;
        }
        troopInfos.text = $"Troop: {card.CardName} \nCard Damage: {card.CardDamage} \nCard Walk Distance: {card.CardWalkDistance} \nCard Attack Distance: {card.CardAttackDistance}";
    }

    // Função para redefinir a interface das cartas na mão do jogador
    public void ResetHandCardsUI() {
        for (int i = 0; i < handObjects.Count; i++)
            Destroy(handObjects[i]);
        handObjects.Clear();
    }

    // Função para adicionar uma carta à mão do jogador na interface
    public void GetCardToHand(CardScriptable card) {
        GameObject _card = Instantiate(cardBase, handTransform);
        _card.GetComponent<CardUIBase>().card = card;
        _card.GetComponent<CardUIBase>().SetupCardUI();
        handObjects.Add(_card);
    }
}
