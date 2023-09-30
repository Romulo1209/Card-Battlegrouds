using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : PlayerBase
{
    [SerializeField] LayerMask gridLayer; 

    [SerializeField] CardUIBase actualCardUI;

    public CardUIBase ActualCardUI { 
        set {
            if(actualCardUI != null)
                actualCardUI.UnselectCard();
            actualCardUI = value;
            if(value != null)
                actualCardUI.SelectCard();
        } 
    }

    public void Update() {
        GetCells();
    }

    void GetCells() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, gridLayer)) {
            CellGrid cellGrid = hit.collider.GetComponent<CellGrid>();
            if (cellGrid == null)
                return;

            if (CurrentCell != null) CurrentCell.OnUnhover();
            CurrentCell = cellGrid;
            CurrentCell.OnHover();
            if (Input.GetMouseButton(0)) CurrentCell.OnClick();

            return;
        }
    }

    public override void SelectCard(CardScriptable _card, CardUIBase cardUI = null) {
        if (CurrentCard != null && CurrentCard == _card) {
            CurrentCard = null;
            ActualCardUI = null;
            return;
        }
        CurrentCard = _card;
        ActualCardUI = cardUI;
    }
    public override void SetupPlayer() {
        base.SetupPlayer();
        DrawHandCards();
    }

    public void DrawHandCards() {
        foreach(CardScriptable card in ActualDeck) {
            GameController.instance.UIController.GetCardToHand(card);
        }
    }
}
