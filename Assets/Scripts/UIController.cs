using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Main UI References")]
    [SerializeField] TMP_Text playerTurnText;
    [SerializeField] TMP_Text manaText;
    [SerializeField] TMP_Text roundText;
    [SerializeField] TMP_Text troopInfos;
    [SerializeField] Transform handTransform;

    [Header("Game Over References")]
    [SerializeField] TMP_Text headerText;
    [SerializeField] TMP_Text highScore;
    [SerializeField] TMP_Text scoreText;

    [SerializeField] GameObject cardBase;

    [SerializeField] List<WindowBase> windows;

    List<GameObject> handObjects = new List<GameObject>();

    public void UpdateHUD(int actualMana, int actualRound, Players playerTurn) {
        playerTurnText.text = playerTurn == Players.Player ? "Player 1 Turn" : "Player 2 Turn";
        manaText.text = $"Mana \n{actualMana}";
        roundText.text = $"Round \n{actualRound}";
    }

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

    public void SetTroopInfos(CardScriptable card) {
        if (card == null) {
            troopInfos.text = null;
            return;
        }
        troopInfos.text = $"Troop: {card.CardName} \nCard Damage: {card.CardDamage} \nCard Card Walk Distance: {card.CardWalkDistance} \nCard Attack Distance: {card.CardAttackDistance}";
    }

    public void ResetHandCardsUI() {
        for (int i = 0; i < handObjects.Count; i++)
            Destroy(handObjects[i]);
        handObjects.Clear();
    }

    public void GetCardToHand(CardScriptable card) {
        GameObject _card = Instantiate(cardBase, handTransform);
        _card.GetComponent<CardUIBase>().card = card;
        _card.GetComponent<CardUIBase>().SetupCardUI();
        handObjects.Add(_card);
    }
}
