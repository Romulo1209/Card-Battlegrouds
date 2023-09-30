using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGrid : MonoBehaviour, IInteractable
{
    [SerializeField] CellGridType cellType; // O tipo de célula (jogador, inimigo ou neutro)
    [SerializeField] Vector2 cellPosition; // A posição da célula na grade
    [SerializeField] Transform cubeStandTransform; // A posição de elevação da célula para personagens

    CharacterBase actualCharacterBase; // O personagem atualmente na célula
    CardScriptable actualCard; // A carta atualmente na célula

    Material cellMaterial; // Material usado para controlar a cor da célula

    #region Getters && Setters 

    public CellGridType CellType { get => cellType; } // Getter para o tipo de célula
    public Vector2 CellPosition { get => cellPosition; } // Getter para a posição da célula
    public Transform CubeStandTransform { get => cubeStandTransform; } // Getter para a posição de elevação da célula
    public CardScriptable ActualCard { get => actualCard; set => actualCard = value; } // Getter e Setter para a carta atualmente na célula
    public CharacterBase ActualCharacterBase { get => actualCharacterBase; set => actualCharacterBase = value; } // Getter e Setter para o personagem atualmente na célula
    Material CellMaterial {
        get => cellMaterial;
        set {
            GetComponent<MeshRenderer>().material = value;
            cellMaterial = value;
        }
    }

    #endregion

    public void InitCell(CellGridType _cellType, Vector2 _cellPosition) {
        this.cellType = _cellType; // Inicializa o tipo de célula com o valor fornecido
        this.cellPosition = _cellPosition; // Inicializa a posição da célula com o valor fornecido
        this.CellMaterial = new Material(GetComponent<MeshRenderer>().material); // Inicializa o material da célula com base no material atual

        CellMaterial.SetColor("_GridColor", GetTeamColor()); // Define a cor da célula com base no tipo de equipe
    }

    public void FillCell(CardScriptable card, CharacterBase cardGameObject) {
        ActualCard = card; // Preenche a célula com uma carta
        ActualCharacterBase = cardGameObject; // Preenche a célula com um personagem
    }

    public void ClearCell() {
        ActualCard = null; // Limpa a célula da carta
        ActualCharacterBase = null; // Limpa a célula do personagem
    }

    #region Mouse Functions

    void Click() {
        if (ActualCard != null)
            return;

        GameController.instance.SpawnCharacter(this, GameController.instance.PlayerController.CurrentCard, Players.Player);
    }

    void Hover() {
        CellMaterial.SetColor("_GridColor", GetHoverColor()); // Define a cor da célula para a cor de destaque
        GameController.instance.UIController.SetTroopInfos(ActualCard); // Atualiza as informações da unidade exibidas na UI
    }

    void Unhover() {
        CellMaterial.SetColor("_GridColor", GetTeamColor()); // Retorna a cor da célula à cor da equipe
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

    public void OnClick() => Click(); // Implementação da interface IInteractable para cliques

    public void OnHover() => Hover(); // Implementação da interface IInteractable para passar o mouse por cima

    public void OnUnhover() => Unhover(); // Implementação da interface IInteractable para sair do mouse por cima

    #endregion
}

public enum CellGridType { Player, Enemy, Neutral } // Enumeração para os tipos de células (jogador, inimigo, neutro)
