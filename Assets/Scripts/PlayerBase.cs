using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [Header("Player Parameters")]
    [SerializeField] int initialMana = 3;
    [SerializeField] int actualMana = 3;
    [SerializeField] int manaLimit = 10;

    [Header("Deck")]
    [SerializeField] List<CardScriptable> actualDeck;

    private CardScriptable currentCard;
    private CellGrid currentCell;

    #region Getters && Setters

    public int Mana { get => actualMana; set { if (value >= manaLimit) value = manaLimit; actualMana = value; } }
    public int MaxMana { get => manaLimit; }

    public CardScriptable CurrentCard { get => currentCard; set => currentCard = value; }
    public CellGrid CurrentCell { get => currentCell; set => currentCell = value; }

    public List<CardScriptable> ActualDeck { get => actualDeck; set => actualDeck = value; }

    #endregion

    public virtual void SelectCard(CardScriptable _card, CardUIBase cardUI = null) {
        if (CurrentCard != null && CurrentCard == _card) {
            CurrentCard = null;
            return;
        }
        CurrentCard = _card;
    }

    public virtual void SetupPlayer() {
        Mana = initialMana;
    }

    public void NextRound() {
        Mana = Mana + 1;
    }
}
