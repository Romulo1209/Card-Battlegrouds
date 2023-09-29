using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGrid : MonoBehaviour, IInteractable
{
    [SerializeField] CellGridType cellType;
    [SerializeField] Vector2 cellPosition;
    [SerializeField] Transform cubeStandTransform;

    CharacterBase actualCharacterBase;
    CardScriptable actualCard;

    Material cellMaterial;

    #region Getters && Setters 

    public CellGridType CellType { get => cellType; }
    public Vector2 CellPosition { get => cellPosition; }
    public Transform CubeStandTransform { get => cubeStandTransform; }
    public CardScriptable ActualCard { get => actualCard; set => actualCard = value; }
    public CharacterBase ActualCharacterBase { get => actualCharacterBase; set => actualCharacterBase = value; }
    Material CellMaterial { 
        get => cellMaterial;
        set {
            GetComponent<MeshRenderer>().material = value;
            cellMaterial = value;
        }
    }

    #endregion

    public void InitCell(CellGridType _cellType, Vector2 _cellPosition) {
        this.cellType = _cellType;
        this.cellPosition = _cellPosition;
        this.CellMaterial = new Material(GetComponent<MeshRenderer>().material);

        CellMaterial.SetColor("_GridColor", GetTeamColor());
    }

    public void FillCell(CardScriptable card, CharacterBase cardGameObject) {
        ActualCard = card;
        ActualCharacterBase = cardGameObject;
    }
    public void ClearCell() {
        ActualCard = null;
        ActualCharacterBase = null;
    }

    #region Mouse Functions

    void Click() {
        if (ActualCard != null)
            return;

        GameController.instance.SpawnCharacter(this, GameController.instance.PlayerController.CurrentCard, Players.Player);
    }
    void Hover() {
        CellMaterial.SetColor("_GridColor", GetHoverColor());
        GameController.instance.UIController.SetTroopInfos(ActualCard);
    }

    void Unhover() {
        CellMaterial.SetColor("_GridColor", GetTeamColor());
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

    public void OnClick() => Click();

    public void OnHover() => Hover();

    public void OnUnhover() => Unhover();

    #endregion
}

public enum CellGridType { Player, Enemy, Neutral }
