using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Players { Player, Bot } // Enum para representar os jogadores
public enum GameState { Play, GameOver } // Enum para representar os estados do jogo

public class GameController : MonoBehaviour {
    // Campos serializados visíveis no Unity Inspector

    [Header("Game Informations")]
    [SerializeField] GameState actualState; // Estado atual do jogo
    [SerializeField] Players nextToPlay; // Próximo jogador a jogar
    [SerializeField] int round; // Número da rodada atual
    [SerializeField] List<CharacterBase> queueOrder; // Lista de personagens em ordem

    [Header("Player 1")]
    [SerializeField][Range(0, 15)] int player1Life = 15; // Vida do jogador 1 (limitada entre 0 e 15)
    [SerializeField] List<CharacterBase> playerCharacters; // Lista de personagens do jogador 1

    [Header("Player 2")]
    [SerializeField][Range(0, 15)] int player2Life = 15; // Vida do jogador 2 (Bot) (limitada entre 0 e 15)
    [SerializeField] List<CharacterBase> enemyCharacters; // Lista de personagens do jogador 2 (Bot)

    [Header("References")]
    [SerializeField] Transform charactersFather; // Transform para organizar personagens

    [Header("Scripts References")]
    [SerializeField] GridController gridController; // Referência ao controlador da grade do jogo
    [SerializeField] UIController uiController; // Referência ao controlador da interface do usuário
    [SerializeField] WindowController windowController; // Referência ao controlador de janelas
    [SerializeField] PlayerController playerController; // Referência ao controlador do jogador
    [SerializeField] BotController botController; // Referência ao controlador do Bot

    bool runningRoundLoop = false; // Variável para controlar se o loop de rodadas está em execução
    private int score; // Pontuação atual
    private int highScore; // Pontuação mais alta

    public static GameController instance; // Instância estática do GameController acessível globalmente

    #region Getters && Setters
    // Propriedades (Getters & Setters) para acessar campos privados

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
        instance = this; // Define a instância estática para permitir acesso global
    }

    private void Start() {
        SetupGame(); // Inicializa o jogo no início
    }

    #region Game Setup and Round Management

    // Métodos relacionados à configuração do jogo e à gestão das rodadas

    void SetupGame() {
        Round = 1; // Inicializa o número da rodada
        player1Life = 15; // Inicializa a vida do jogador 1
        player2Life = 15; // Inicializa a vida do jogador 2 (Bot)
        highScore = PlayerPrefs.GetInt("HighScore"); // Obtém a pontuação mais alta do jogador anterior

        GridController.SetupGrid(); // Configura a grade do jogo
        PlayerController.SetupPlayer(); // Configura o jogador
        BotController.SetupPlayer(); // Configura o Bot
        UIController.UpdateHUD(PlayerController.Mana, Round, nextToPlay); // Atualiza a interface do usuário
        windowController.OpenWindow("Main UI"); // Abre a janela principal

        ActualState = GameState.Play; // Define o estado atual como "Play"
    }

    public void Menu() {
        SceneManager.LoadSceneAsync(0); // Carrega a cena do menu
    }

    public void ResetGame() {
        GridController.RemoveActualGrid(); // Remove a grade atual
        UIController.ResetHandCardsUI(); // Reseta a interface de cartas na mão
        RemoveAllCharacters(); // Remove todos os personagens
        SetupGame(); // Inicializa um novo jogo
    }

    public void NextRound(bool button = false) {
        // Avança para a próxima rodada, lidando com as lógicas de troca de turno entre jogador e Bot
        if (runningRoundLoop || button && nextToPlay == Players.Bot)
            return;

        if (nextToPlay == Players.Player) {
            nextToPlay = Players.Bot;
            BotController.BotRound(); // Rodada do Bot

            UIController.UpdateHUD(PlayerController.Mana, Round, nextToPlay);
            return;
        }

        StartCoroutine(RoundLoop()); // Inicia o loop de rodadas
    }

    void PassRound() {
        Round++; // Incrementa o número da rodada
        nextToPlay = Players.Player; // Define o próximo jogador como o jogador

        PlayerController.NextRound(); // Configura o próximo turno do jogador
        BotController.NextRound(); // Configura o próximo turno do Bot
        UIController.UpdateHUD(PlayerController.Mana, Round, nextToPlay); // Atualiza a interface do usuário
    }

    void CheckWinner() {
        // Verifica se há um vencedor com base na quantidade de vida dos jogadores
        if (player1Life > 0 && player2Life > 0)
            return;

        ActualState = GameState.GameOver; // Define o estado atual como "GameOver"

        if (player1Life <= 0)
            score = 0;
        else
            score = GetScore(); // Calcula a pontuação

        Players winner = player1Life > 0 ? Players.Player : Players.Bot; // Determina o vencedor
        UIController.SetGameOverWindow(winner, score); // Configura a janela de "Game Over"
        windowController.OpenWindow("Game Over"); // Abre a janela de "Game Over"
    }

    int GetScore() {
        // Calcula a pontuação do jogo com base em várias condições e a armazena
        int score = (100 - Round);
        score = score <= 0 ? 0 : score;
        score += 30;

        if (score > PlayerPrefs.GetInt("HighScore"))
            PlayerPrefs.SetInt("HighScore", score); // Atualiza a pontuação mais alta se necessário

        return score;
    }

    void RemoveAllCharacters() {
        // Remove todos os personagens do jogo
        for (int i = 0; i < queueOrder.Count; i++)
        {
            if (queueOrder[i] != null)
                Destroy(queueOrder[i].gameObject);
        }
        playerCharacters.Clear();
        enemyCharacters.Clear();
        queueOrder.Clear();
    }



    public int GetPlayerLife(Players player) {
        // Obtém a quantidade de vida de um jogador específico
        switch (player)
        {
            case Players.Player:
                return player1Life;
            case Players.Bot:
                return player2Life;
        }
        return 0;
    }

    #endregion

    #region Battlefield Functions
    // Funções relacionadas ao campo de batalha

    public void DamageBase(Players sideToDamage, int damage) {
        // Aplica dano à base de um jogador específico
        switch (sideToDamage) {
            case Players.Player:
                player2Life -= damage;
                player2Life = player2Life < 0 ? 0 : player2Life; // Garante que a vida não seja negativa
                break;
            case Players.Bot:
                player1Life -= damage;
                player1Life = player1Life < 0 ? 0 : player1Life; // Garante que a vida não seja negativa
                break;
        }

        for (int i = 0; i < GridController.Castles.Count; i++)
        {
            GridController.Castles[i].UpdateLife(); // Atualiza a vida das construções no campo de batalha
        }
    }

    public void SpawnCharacter(CellGrid cell, CardScriptable currentCard, Players spawnerOwner) {
        // Instancia um personagem no campo de batalha
        var mana = spawnerOwner == Players.Player ? PlayerController.Mana : BotController.Mana;
        if (currentCard == null || !SpawnInRightCell(spawnerOwner, cell) || mana < currentCard.CardCost || cell.ActualCard != null || spawnerOwner != nextToPlay) {
            // Verifica várias condições antes de instanciar um personagem

            if (spawnerOwner == Players.Player) {
                PlayerController.CurrentCard = null;
                PlayerController.ActualCardUI = null;
            }
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
            PlayerController.ActualCardUI = null;
            playerCharacters.Add(character.GetComponent<CharacterBase>());
        }
        else {
            character.GetComponent<CharacterBase>().StartCharacter(false);
            BotController.Mana = BotController.Mana - currentCard.CardCost;
            enemyCharacters.Add(character.GetComponent<CharacterBase>());
        }
    }

    public IEnumerator RoundLoop() {
        // Loop que gerencia as rodadas
        runningRoundLoop = true;

        playerCharacters = CheckCharacters(playerCharacters);
        enemyCharacters = CheckCharacters(enemyCharacters);
        queueOrder = CheckCharacters(queueOrder);

        if (queueOrder == null)
            yield return null;

        for (int i = 0; i < queueOrder.Count; i++)
        {
            var character = queueOrder[i];
            if (!character.Alive || character == null)
                continue;

            bool attacked = character.AttackCharacter();
            if (!attacked)
                character.MoveCharacter();

            yield return new WaitForSeconds(0.25f);
        }

        runningRoundLoop = false;
        CheckWinner();
        PassRound();
    }

    List<CharacterBase> CheckCharacters(List<CharacterBase> list) {
        // Remove personagens mortos ou nulos da lista de personagens
        if (list == null)
            return null;

        for (int i = 0; i < list.Count; i++) {
            if (!list[i].Alive || list[i] == null) {
                list.RemoveAt(i);
                i--;
            }
        }
        return list;
    }

    bool SpawnInRightCell(Players owner, CellGrid cell) {
        // Verifica se um personagem pode ser instanciado em uma célula específica
        return owner == Players.Player && cell.CellType == CellGridType.Player || owner == Players.Bot && cell.CellType == CellGridType.Enemy ? true : false;
    }

    #endregion
}
