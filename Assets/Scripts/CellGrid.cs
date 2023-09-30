using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGrid : MonoBehaviour, IInteractable
{
    [SerializeField] CellGridType cellType; // O tipo de c�lula (jogador, inimigo ou neutro)
    [SerializeField] Vector2 cellPosition; // A posi��o da c�lula na grade
    [SerializeField] Transform cubeStandTransform; // A posi��o de eleva��o da c�lula para personagens

    CharacterBase actualCharacterBase; // O personagem atualmente na c�lula
    CardScriptable actualCard; // A carta atualmente na c�lula

    Material cellMaterial; // Material usado para controlar a cor da c�lula

    #region Getters && Setters 

    public CellGridType CellType { get => cellType; } // Getter para o tipo de c�lula
    public Vector2 CellPosition { get => cellPosition; } // Getter para a posi��o da c�lula
    public Transform CubeStandTransform { get => cubeStandTransform; } // Getter para a posi��o de eleva��o da c�lula
    public CardScriptable ActualCard { get => actualCard; set => actualCard = value; } // Getter e Setter para a carta atualmente na c�lula
    public CharacterBase ActualCharacterBase { get => actualCharacterBase; set => actualCharacterBase = value; } // Getter e Setter para o personagem atualmente na c�lula
    Material CellMaterial {
        get => cellMaterial;
        set {
            GetComponent<MeshRenderer>().material = value;
            cellMaterial = value;
        }
    }

    #endregion

    public void InitCell(CellGridType _cellType, Vector2 _cellPosition) {
        this.cellType = _cellType; // Inicializa o tipo de c�lula com o valor fornecido
        this.cellPosition = _cellPosition; // Inicializa a posi��o da c�lula com o valor fornecido
        this.CellMaterial = new Material(GetComponent<MeshRenderer>().material); // Inicializa o material da c�lula com base no material atual

        CellMaterial.SetColor("_GridColor", GetTeamColor()); // Define a cor da c�lula com base no tipo de equipe
    }

    public void FillCell(CardScriptable card, CharacterBase cardGameObject) {
        ActualCard = card; // Preenche a c�lula com uma carta
        ActualCharacterBase = cardGameObject; // Preenche a c�lula com um personagem
    }

    public void ClearCell() {
        ActualCard = null; // Limpa a c�lula da carta
        ActualCharacterBase = null; // Limpa a c�lula do personagem
    }

    #region Mouse Functions

    void Click() {
        if (ActualCard != null)
            return;

        GameController.instance.SpawnCharacter(this, GameController.instance.PlayerController.CurrentCard, Players.Player);
    }

    void Hover() {
        CellMaterial.SetColor("_GridColor", GetHoverColor()); // Define a cor da c�lula para a cor de destaque
        GameController.instance.UIController.SetTroopInfos(ActualCard); // Atualiza as informa��es da unidade exibidas na UI
    }

    void Unhover() {
        CellMaterial.SetColor("_GridColor", GetTeamColor()); // Retorna a cor da c�lula � cor da equipe
    }

    #endregion

    #region Returns

    Color GetTeamColor() {
        switch (CellType) {
            case CellGridType.Player: return GameController.instance.GridController.CellColors.Player;
            case CellGridType.Enemy: return GameController.instance.GridController.CellColors.Enemy;
            default: return GameController.instance.GridController.CellColors.Neutral;
        }
    }

    Color GetHoverColor() {
        switch (CellType) {
            case CellGridType.Player: return GameController.instance.GridController.CellColors.PlayerHoverTiles;
            case CellGridType.Enemy: return GameController.instance.GridController.CellColors.EnemyHoverTiles;
            default: return GameController.instance.GridController.CellColors.NeutralHoverColor;
        }
    }

    #endregion

    #region Interface

    public void OnClick() => Click(); // Implementa��o da interface IInteractable para cliques

    public void OnHover() => Hover(); // Implementa��o da interface IInteractable para passar o mouse por cima

    public void OnUnhover() => Unhover(); // Implementa��o da interface IInteractable para sair do mouse por cima

    #endregion
}

public enum CellGridType { Player, Enemy, Neutral } // Enumera��o para os tipos de c�lulas (jogador, inimigo, neutro)
