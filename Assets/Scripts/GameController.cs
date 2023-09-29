using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Players { Player, Bot }
public enum GameState { Play, GameOver}

public class GameController : MonoBehaviour
{
    [Header("Game Informations")]
    [SerializeField] GameState actualState;
    [SerializeField] Players nextToPlay;
    [SerializeField] int round;
    [SerializeField] List<CharacterBase> queueOrder;

    [Header("Player 1")]
    [SerializeField] [Range(0, 15)] int player1Life = 15;
    [SerializeField] List<CharacterBase> playerCharacters;

    [Header("Player 2")]
    [SerializeField][Range(0, 15)] int player2Life = 15;
    [SerializeField] List<CharacterBase> enemyCharacters;

    [Header("References")]
    [SerializeField] Transform charactersFather;

    [Header("Scripts References")]
    [SerializeField] GridController gridController;
    [SerializeField] UIController uiController;
    [SerializeField] WindowController windowController;
    [SerializeField] PlayerController playerController;
    [SerializeField] BotController botController;

    bool runningRoundLoop = false;
    private int score;
    private int highScore;

    public static GameController instance;

    #region Getters && Setters

    public List<CharacterBase> PlayerCharacters { get => playerCharacters; }

    public GameState ActualState { get => actualState; set => actualState = value; }
    public int Round { get => round; set => round = value; }

    public GridController GridController { get => gridController; }
    public UIController UIController { get => uiController; }
    public WindowController WindowController { get => windowController; }
    public PlayerController PlayerController { get => playerController; }
    public BotController BotController { get => botController; }

    #endregion

    private void Awake() {
        instance = this;
    }

    private void Start() {
        SetupGame();
    }

    #region Game Setup and Round Management

    void SetupGame() {
        Round = 1;
        player1Life = 15;
        player2Life = 15;
        highScore = PlayerPrefs.GetInt("HighScore");

        GridController.SetupGrid();
        PlayerController.SetupPlayer();
        BotController.SetupPlayer();
        UIController.UpdateHUD(PlayerController.Mana, Round, nextToPlay);
        windowController.OpenWindow("Main UI");

        ActualState = GameState.Play;
    }

    public void Menu() {
        SceneManager.LoadSceneAsync(0);
    }

    public void ResetGame() {
        GridController.RemoveActualGrid();
        UIController.ResetHandCardsUI();
        RemoveAllCharacters();
        SetupGame();
    }

    public void NextRound(bool button = false) {
        if (runningRoundLoop || button && nextToPlay == Players.Bot)
            return;

        if(nextToPlay == Players.Player) {
            nextToPlay = Players.Bot;
            BotController.BotRound();

            UIController.UpdateHUD(PlayerController.Mana, Round, nextToPlay);
            return;
        }

        StartCoroutine(RoundLoop());
    }

    void PassRound() {
        Round++;
        nextToPlay = Players.Player;

        PlayerController.NextRound();
        BotController.NextRound();
        UIController.UpdateHUD(PlayerController.Mana, Round, nextToPlay);
    }

    void CheckWinner() {
        if (player1Life > 0 && player2Life > 0)
            return;

        ActualState = GameState.GameOver;

        if(player1Life <= 0) score = 0;
        else score = GetScore();

        Players winner = player1Life > 0 ? Players.Player : Players.Bot;
        UIController.SetGameOverWindow(winner, score);
        windowController.OpenWindow("Game Over");
    }

    int GetScore() {
        int score = (100 - Round);
        score = score <= 0 ? 0 : score;
        score += 30;

        if (score > PlayerPrefs.GetInt("HighScore"))
            PlayerPrefs.SetInt("HighScore", score);

        return score;
    }

    void RemoveAllCharacters() {
        for(int i = 0; i < queueOrder.Count; i++) {
            if (queueOrder[i] != null)
                Destroy(queueOrder[i].gameObject);
        }
        playerCharacters.Clear();
        enemyCharacters.Clear();
        queueOrder.Clear();
    }

    public int GetPlayerLife(Players player) {
        switch (player) {
            case Players.Player:
                return player1Life;
            case Players.Bot:
                return player2Life; ;
        }
        return 0;
    }

    #endregion

    #region Battlefield Functions

    public void DamageBase(Players sideToDamage, int damage) {
        switch (sideToDamage) {
            case Players.Player:
                player2Life -= damage;
                player2Life = player2Life < 0 ? 0 : player2Life;
                break;
            case Players.Bot:
                player1Life -= damage;
                player1Life = player1Life < 0 ? 0 : player1Life;
                break;
        }

        for(int i = 0; i < GridController.Castles.Count; i++) {
            GridController.Castles[i].UpdateLife();
        }
    }

    public void SpawnCharacter(CellGrid cell, CardScriptable currentCard, Players spawnerOwner) {
        var mana = spawnerOwner == Players.Player ? PlayerController.Mana : BotController.Mana;
        if (currentCard == null || !SpawnInRightCell(spawnerOwner, cell) || mana < currentCard.CardCost || cell.ActualCard != null || spawnerOwner != nextToPlay) {
            if(spawnerOwner == Players.Player) 
                PlayerController.CurrentCard = null;
            return;
        } 

        GameObject character = Instantiate(currentCard.CharacterPrefab, cell.CubeStandTransform.position, Quaternion.identity);
        character.GetComponent<CharacterBase>().ActualCellGrid = cell;
        character.transform.SetParent(charactersFather);
        cell.FillCell(currentCard, character.GetComponent<CharacterBase>());
        queueOrder.Add(character.GetComponent<CharacterBase>());
        if (spawnerOwner == Players.Player) {
            character.GetComponent<CharacterBase>().StartCharacter(true);
            PlayerController.Mana = PlayerController.Mana - currentCard.CardCost;
            UIController.UpdateHUD(PlayerController.Mana, GameController.instance.Round, nextToPlay);
            PlayerController.CurrentCard = null;
            playerCharacters.Add(character.GetComponent<CharacterBase>());
        }
        else {
            character.GetComponent<CharacterBase>().StartCharacter(false);
            BotController.Mana = BotController.Mana - currentCard.CardCost;
            enemyCharacters.Add(character.GetComponent<CharacterBase>());
        }
    }

    public IEnumerator RoundLoop() {
        runningRoundLoop = true;

        playerCharacters = CheckCharacters(playerCharacters);
        enemyCharacters = CheckCharacters(enemyCharacters);
        queueOrder = CheckCharacters(queueOrder);

        if (queueOrder == null)
            yield return null;

        for (int i = 0; i < queueOrder.Count; i++) {
            var character = queueOrder[i];
            if (!character.Alive || character == null) 
                continue;

            bool attacked = character.AttackCharacter();
            if(!attacked)
                character.MoveCharacter();

            yield return new WaitForSeconds(0.25f);
        }

        runningRoundLoop = false;
        CheckWinner();
        PassRound();
    }

    List<CharacterBase> CheckCharacters(List<CharacterBase> list) {
        if (list == null)
            return null;

        for(int i = 0; i < list.Count; i++) {
            if (!list[i].Alive || list[i] == null) {
                list.RemoveAt(i);
                i--;
            }
        }
        return list;
    }

    bool SpawnInRightCell(Players owner, CellGrid cell) {
        return owner == Players.Player && cell.CellType == CellGridType.Player || owner == Players.Bot && cell.CellType == CellGridType.Enemy ? true : false; 
    }

    #endregion
}