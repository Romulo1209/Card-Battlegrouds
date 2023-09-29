using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;
using static CharacterBase;

public class CharacterBase : MonoBehaviour
{
    [Header("Character Infos")]
    [SerializeField] Players characterOwner;
    [SerializeField] CardScriptable cardScriptable;
    [SerializeField] bool alive = true;

    [Header("References")]
    [SerializeField] int characterMaxLife;
    [SerializeField] int characterLife;
    [SerializeField] int characterDamage;
    [SerializeField] int characterWalkDistance;
    [SerializeField] int characterAttackDistance;
    [SerializeField] float characterMoveSpeed = 3;

    [Header("Objects References")]
    [SerializeField] Animator animator;
    [SerializeField] Transform characterTransform;
    [SerializeField] CellGrid actualCellGrid;
    [SerializeField] GameObject hitParticle;

    [Header("UI References")]
    [SerializeField] Image lifeFill;
    [SerializeField] TMP_Text lifeText;

    bool moving;

    #region Getters && Setters

    public int CharacterLife { get => characterLife; set { if (value < 0) value = 0; characterLife = value;  } }

    public Players Owner { get => characterOwner; }
    public bool Alive { get => alive; set => alive = value; }
    public bool PlayerCharacter { get => PlayerCharacter; }
    public CellGrid ActualCellGrid { get => actualCellGrid; set => actualCellGrid = value; }

    #endregion

    public virtual void StartCharacter(bool playerChar) {
        characterMaxLife = cardScriptable.CardHealth;
        characterLife = cardScriptable.CardHealth;
        characterDamage = cardScriptable.CardDamage;
        characterWalkDistance = cardScriptable.CardWalkDistance;
        characterAttackDistance = cardScriptable.CardAttackDistance;

        if (playerChar) {
            characterOwner = Players.Player;
            characterTransform.Rotate(new Vector3(0, 90, 0), Space.Self);
        } 
        else {
            characterOwner = Players.Bot;
            characterTransform.Rotate(new Vector3(0, -90, 0), Space.Self);
        } 

        UpdateCharacter();
    }

    public virtual void UpdateCharacter() {
        lifeFill.fillAmount = (float)CharacterLife / (float)characterMaxLife;
        lifeText.text = $"{(float)CharacterLife}/{(float)characterMaxLife}";
    }

    public virtual void MoveCharacter() {
        for(int i = 0; i < characterWalkDistance; i++) {
            CellGrid nextCell;
            if (characterOwner == Players.Player) nextCell = GameController.instance.GridController.GetAroundCellGrid(actualCellGrid, 1, GetDirection.Right);
            else nextCell = GameController.instance.GridController.GetAroundCellGrid(actualCellGrid, 1, GetDirection.Left);

            if (nextCell == null || nextCell.ActualCard != null) {
                if (nextCell == null) {
                    GameController.instance.DamageBase(characterOwner, characterDamage);
                    actualCellGrid.ClearCell();
                    Destroy(gameObject);
                }
                return;
            }
            actualCellGrid.ClearCell();
            actualCellGrid = nextCell;
            actualCellGrid.FillCell(cardScriptable, this);
            StopAllCoroutines();
            StartCoroutine(Move(actualCellGrid.CubeStandTransform.position));
        }
    }

    public virtual bool AttackCharacter() {
        for(int i = 1; i < characterAttackDistance + 1; i++) {
            CellGrid cellToAttack;
            if (characterOwner == Players.Player) cellToAttack = GameController.instance.GridController.GetAroundCellGrid(actualCellGrid, i, GetDirection.Right);
            else cellToAttack = GameController.instance.GridController.GetAroundCellGrid(actualCellGrid, i, GetDirection.Left);

            if (cellToAttack == null || cellToAttack.ActualCharacterBase == null || cellToAttack.ActualCharacterBase.Owner == Owner)
                continue;

            cellToAttack.ActualCharacterBase.TakeDamage(this, cellToAttack);
            animator.SetTrigger("Attack");
            return true;
        }
        return false;
    }

    public void TakeDamage(CharacterBase damageDealer, CellGrid cell) {
        CharacterLife -= damageDealer.characterDamage;
        Instantiate(hitParticle, transform.position, Quaternion.identity);
        UpdateCharacter();
        if (CharacterLife <= 0) {
            StartCoroutine(Die(cell));
            return;
        }
        animator.SetTrigger("Hit");
    }

    IEnumerator Move(Vector3 position) {
        moving = true;
        while (transform.position != position) {
            animator.SetBool("Walking", true);
            transform.position = Vector3.MoveTowards(transform.position, position, characterMoveSpeed * Time.fixedDeltaTime);
            yield return null;
        }
        animator.SetBool("Walking", false);
        moving = false;
    }
    IEnumerator Die(CellGrid cell) {
        cell.ClearCell();
        Alive = false;
        animator.SetTrigger("Die");

        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
}
