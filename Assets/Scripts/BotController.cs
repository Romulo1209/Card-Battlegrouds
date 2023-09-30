using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BotController : PlayerBase
{
    int saveManaPercentBase = 30; // Percentual base de economia de mana para o bot
    int saveManaPercent = 30; // Percentual atual de economia de mana para o bot

    // Função para executar o turno do bot
    public void BotRound() {
        List<CardScriptable> cardsAvailable = GetCardsCanUse(); // Obtém as cartas que o bot pode usar no turno atual
        int number = Random.Range(0, 100); // Gera um número aleatório entre 0 e 100

        // Decide se o bot jogará uma carta neste turno com base nas cartas disponíveis e no número aleatório gerado
        if (!PlayNextRound(cardsAvailable.Count, number)) {
            // Se o bot não jogar uma carta neste turno, aumenta o percentual de economia de mana
            saveManaPercent += cardsAvailable.Count == 0 ? 5 : 10;
        }
        else {
            // Se o bot decidir jogar uma carta, reseta o percentual de economia de mana e invoca a função para escolher a linha e a carta para jogar
            saveManaPercent = saveManaPercentBase;
            GameController.instance.SpawnCharacter(ChooseLineToSpawn(), ChooseCardToSpawn(cardsAvailable), Players.Bot);
        }
        StartCoroutine(EndRound()); // Inicia a rotina para encerrar o turno do bot após um curto atraso
    }

    // Função para decidir se o bot jogará uma carta no próximo turno
    bool PlayNextRound(int cardsAvailable, int playChance) {
        if (cardsAvailable == 0 || playChance > saveManaPercent) {
            // Se não houver cartas disponíveis ou a chance de jogar for menor que o percentual de economia de mana, o bot não joga uma carta
            foreach (CharacterBase character in GameController.instance.PlayerCharacters) {
                // Verifica se os personagens do jogador estão a uma certa distância antes de decidir não jogar uma carta
                if (GameController.instance.GridController.GridSize.x - character.ActualCellGrid.CellPosition.x <= (int)(GameController.instance.GridController.GridSize.x / 2)) {
                    return true; // Retorna verdadeiro se o jogador estiver próximo
                }
            }
            return false; // Retorna falso se o jogador estiver longe
        }
        return true; // Retorna verdadeiro se o bot decidir jogar uma carta
    }

    // Função para escolher uma carta para jogar aleatoriamente entre as disponíveis
    CardScriptable ChooseCardToSpawn(List<CardScriptable> cardsAvailable) {
        if (cardsAvailable.Count == 0 || cardsAvailable == null)
            return null;
        return cardsAvailable[Random.Range(0, cardsAvailable.Count)];
    }

    // Função para escolher uma linha no grid para spawn
    CellGrid ChooseLineToSpawn() {
        return GameController.instance.GridController.GetEnemyGrid[Random.Range(0, GameController.instance.GridController.GetEnemyGrid.Count)];
    }

    // Função para obter as cartas que o bot pode usar com base na mana disponível
    List<CardScriptable> GetCardsCanUse() {
        List<CardScriptable> cardsAvailable = new List<CardScriptable>();
        foreach (CardScriptable card in ActualDeck) {
            if (Mana >= card.CardCost)
                cardsAvailable.Add(card);
        }
        return cardsAvailable;
    }

    // Função para encerrar o turno do bot com um pequeno atraso
    public IEnumerator EndRound() {
        yield return new WaitForSeconds(0.5f);
        GameController.instance.NextRound(); // Chama a próxima rodada no GameController
    }
}
