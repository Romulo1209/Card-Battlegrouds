using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BotController : PlayerBase
{
    int saveManaPercentBase = 30; // Percentual base de economia de mana para o bot
    int saveManaPercent = 30; // Percentual atual de economia de mana para o bot

    // Fun��o para executar o turno do bot
    public void BotRound() {
        List<CardScriptable> cardsAvailable = GetCardsCanUse(); // Obt�m as cartas que o bot pode usar no turno atual
        int number = Random.Range(0, 100); // Gera um n�mero aleat�rio entre 0 e 100

        // Decide se o bot jogar� uma carta neste turno com base nas cartas dispon�veis e no n�mero aleat�rio gerado
        if (!PlayNextRound(cardsAvailable.Count, number)) {
            // Se o bot n�o jogar uma carta neste turno, aumenta o percentual de economia de mana
            saveManaPercent += cardsAvailable.Count == 0 ? 5 : 10;
        }
        else {
            // Se o bot decidir jogar uma carta, reseta o percentual de economia de mana e invoca a fun��o para escolher a linha e a carta para jogar
            saveManaPercent = saveManaPercentBase;
            GameController.instance.SpawnCharacter(ChooseLineToSpawn(), ChooseCardToSpawn(cardsAvailable), Players.Bot);
        }
        StartCoroutine(EndRound()); // Inicia a rotina para encerrar o turno do bot ap�s um curto atraso
    }

    // Fun��o para decidir se o bot jogar� uma carta no pr�ximo turno
    bool PlayNextRound(int cardsAvailable, int playChance) {
        if (cardsAvailable == 0 || playChance > saveManaPercent) {
            // Se n�o houver cartas dispon�veis ou a chance de jogar for menor que o percentual de economia de mana, o bot n�o joga uma carta
            foreach (CharacterBase character in GameController.instance.PlayerCharacters) {
                // Verifica se os personagens do jogador est�o a uma certa dist�ncia antes de decidir n�o jogar uma carta
                if (GameController.instance.GridController.GridSize.x - character.ActualCellGrid.CellPosition.x <= (int)(GameController.instance.GridController.GridSize.x / 2)) {
                    return true; // Retorna verdadeiro se o jogador estiver pr�ximo
                }
            }
            return false; // Retorna falso se o jogador estiver longe
        }
        return true; // Retorna verdadeiro se o bot decidir jogar uma carta
    }

    // Fun��o para escolher uma carta para jogar aleatoriamente entre as dispon�veis
    CardScriptable ChooseCardToSpawn(List<CardScriptable> cardsAvailable) {
        if (cardsAvailable.Count == 0 || cardsAvailable == null)
            return null;
        return cardsAvailable[Random.Range(0, cardsAvailable.Count)];
    }

    // Fun��o para escolher uma linha no grid para spawn
    CellGrid ChooseLineToSpawn() {
        return GameController.instance.GridController.GetEnemyGrid[Random.Range(0, GameController.instance.GridController.GetEnemyGrid.Count)];
    }

    // Fun��o para obter as cartas que o bot pode usar com base na mana dispon�vel
    List<CardScriptable> GetCardsCanUse() {
        List<CardScriptable> cardsAvailable = new List<CardScriptable>();
        foreach (CardScriptable card in ActualDeck) {
            if (Mana >= card.CardCost)
                cardsAvailable.Add(card);
        }
        return cardsAvailable;
    }

    // Fun��o para encerrar o turno do bot com um pequeno atraso
    public IEnumerator EndRound() {
        yield return new WaitForSeconds(0.5f);
        GameController.instance.NextRound(); // Chama a pr�xima rodada no GameController
    }
}
