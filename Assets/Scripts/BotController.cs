using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BotController : PlayerBase
{
    int saveManaPercentBase = 30;
    int saveManaPercent = 30;

    public void BotRound() {
        List<CardScriptable> cardsAvailable = GetCardsCanUse();
        int number = Random.Range(0, 100);

        if (!PlayNextRound(cardsAvailable.Count, number)) { saveManaPercent += cardsAvailable.Count == 0 ? 5 : 10; }
        else {
            saveManaPercent = saveManaPercentBase;
            GameController.instance.SpawnCharacter(ChooseLineToSpawn(), ChooseCardToSpawn(cardsAvailable), Players.Bot);
        }
        StartCoroutine(EndRound());
    }

    bool PlayNextRound(int cardsAvailable, int playChance) {
        if (cardsAvailable == 0 || playChance > saveManaPercent) {
            foreach(CharacterBase character in GameController.instance.PlayerCharacters)
                if (GameController.instance.GridController.GridSize.x - character.ActualCellGrid.CellPosition.x <= (int)(GameController.instance.GridController.GridSize.x / 2))
                    return true;
            return false;
        } 
        return true;
    }

    CardScriptable ChooseCardToSpawn(List<CardScriptable> cardsAvailable) {
        if (cardsAvailable.Count == 0 || cardsAvailable == null)
            return null;
        return cardsAvailable[Random.Range(0, cardsAvailable.Count)];
    }
    CellGrid ChooseLineToSpawn() {
        return GameController.instance.GridController.GetEnemyGrid[Random.Range(0, GameController.instance.GridController.GetEnemyGrid.Count)];
    }

    List<CardScriptable> GetCardsCanUse() {
        List<CardScriptable> cardsAvailable = new List<CardScriptable>();
        foreach (CardScriptable card in ActualDeck)
            if (Mana >= card.CardCost)
                cardsAvailable.Add(card);
        return cardsAvailable;
    }

    public IEnumerator EndRound()
    {
        yield return new WaitForSeconds(0.5f);
        GameController.instance.NextRound();
    }
}