using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [Header("Player Parameters")]
    [SerializeField] int initialMana = 3;  // Mana inicial do jogador
    [SerializeField] int actualMana = 3;   // Mana atual do jogador
    [SerializeField] int manaLimit = 10;    // Limite m�ximo de mana que o jogador pode ter

    [Header("Deck")]
    [SerializeField] List<CardScriptable> actualDeck;  // Baralho atual do jogador

    private CardScriptable currentCard;  // Carta atual selecionada pelo jogador
    private CellGrid currentCell;        // C�lula atual selecionada pelo jogador

    #region Getters && Setters

    // Getter e setter para a mana atual do jogador
    public int Mana { get => actualMana; set { if (value >= manaLimit) value = manaLimit; actualMana = value; } }
    // Getter para o limite m�ximo de mana
    public int MaxMana { get => manaLimit; }
    // Getter e setter para a carta atual selecionada pelo jogador
    public CardScriptable CurrentCard { get => currentCard; set => currentCard = value; }
    // Getter e setter para a c�lula atual selecionada pelo jogador
    public CellGrid CurrentCell { get => currentCell; set => currentCell = value; }
    // Getter e setter para o baralho atual do jogador
    public List<CardScriptable> ActualDeck { get => actualDeck; set => actualDeck = value; }

    #endregion

    // M�todo chamado quando uma carta � selecionada
    public virtual void SelectCard(CardScriptable _card, CardUIBase cardUI = null) {
        // Se a carta atual j� estiver selecionada novamente, deseleciona
        if (CurrentCard != null && CurrentCard == _card) {
            CurrentCard = null;
            return;
        }
        // Caso contr�rio, define a carta atual como a carta passada como argumento
        CurrentCard = _card;
    }

    // M�todo chamado para configurar o jogador no in�cio do jogo
    public virtual void SetupPlayer() {
        // Define a mana inicial do jogador
        Mana = initialMana;
    }

    // M�todo chamado no in�cio de cada novo round
    public void NextRound() {
        // Aumenta a mana do jogador em 1
        Mana = Mana + 1;
    }
}
