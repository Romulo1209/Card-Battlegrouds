using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterBase : MonoBehaviour
{
    // Par�metros do personagem
    [Header("Character Infos")]
    [SerializeField] Players characterOwner; // Propriet�rio do personagem (jogador ou IA)
    [SerializeField] CardScriptable cardScriptable; // Informa��es do cart�o associado a este personagem
    [SerializeField] bool alive = true; // Indica se o personagem est� vivo

    // Refer�ncias aos componentes e objetos do personagem
    [Header("References")]
    [SerializeField] int characterMaxLife; // Vida m�xima do personagem
    [SerializeField] int characterLife; // Vida atual do personagem
    [SerializeField] int characterDamage; // Dano do personagem
    [SerializeField] int characterWalkDistance; // Dist�ncia m�xima de movimento do personagem
    [SerializeField] int characterAttackDistance; // Dist�ncia m�xima de ataque do personagem
    [SerializeField] float characterMoveSpeed = 3; // Velocidade de movimento do personagem

    [Header("Objects References")]
    [SerializeField] Animator animator; // Animator para anima��es
    [SerializeField] Transform characterTransform; // Transform do personagem
    [SerializeField] CellGrid actualCellGrid; // C�lula do grid atual onde o personagem est�
    [SerializeField] GameObject hitParticle; // Part�cula de efeito ao ser atacado

    // Refer�ncias � UI
    [Header("UI References")]
    [SerializeField] Image lifeFill; // Barra de vida na UI
    [SerializeField] TMP_Text lifeText; // Texto da vida na UI

    bool moving; // Indica se o personagem est� se movendo

    // Propriedades (Getters e Setters)
    public int CharacterLife { get => characterLife; set { if (value < 0) value = 0; characterLife = value; } }
    public Players Owner { get => characterOwner; }
    public bool Alive { get => alive; set => alive = value; }
    public bool PlayerCharacter { get => PlayerCharacter; }
    public CellGrid ActualCellGrid { get => actualCellGrid; set => actualCellGrid = value; }

    // Inicializa o personagem com base nas informa��es do cart�o
    public virtual void StartCharacter(bool playerChar) {
        // Inicializa par�metros com base nas informa��es do cart�o
        characterMaxLife = cardScriptable.CardHealth;
        characterLife = cardScriptable.CardHealth;
        characterDamage = cardScriptable.CardDamage;
        characterWalkDistance = cardScriptable.CardWalkDistance;
        characterAttackDistance = cardScriptable.CardAttackDistance;

        // Define o propriet�rio do personagem (jogador ou IA)
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

    // Atualiza a representa��o do personagem na UI
    public virtual void UpdateCharacter() {
        lifeFill.fillAmount = (float)CharacterLife / (float)characterMaxLife;
        lifeText.text = $"{(float)CharacterLife}/{(float)characterMaxLife}";
    }

    // Move o personagem
    public virtual void MoveCharacter() {
        // Move o personagem em dire��o � pr�xima c�lula dispon�vel
        for (int i = 0; i < characterWalkDistance; i++) {
            CellGrid nextCell;
            if (characterOwner == Players.Player)
                nextCell = GameController.instance.GridController.GetAroundCellGrid(actualCellGrid, 1, GetDirection.Right);
            else
                nextCell = GameController.instance.GridController.GetAroundCellGrid(actualCellGrid, 1, GetDirection.Left);

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

    // Ataca um personagem
    public virtual bool AttackCharacter() {
        for (int i = 1; i < characterAttackDistance + 1; i++) {
            CellGrid cellToAttack;
            if (characterOwner == Players.Player)
                cellToAttack = GameController.instance.GridController.GetAroundCellGrid(actualCellGrid, i, GetDirection.Right);
            else
                cellToAttack = GameController.instance.GridController.GetAroundCellGrid(actualCellGrid, i, GetDirection.Left);

            if (cellToAttack == null || cellToAttack.ActualCharacterBase == null || cellToAttack.ActualCharacterBase.Owner == Owner)
                continue;

            cellToAttack.ActualCharacterBase.TakeDamage(this, cellToAttack);
            animator.SetTrigger("Attack");
            return true;
        }
        return false;
    }

    // Recebe dano
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

    // Corrutina para mover o personagem suavemente
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

    // Corrutina para lidar com a morte do personagem
    IEnumerator Die(CellGrid cell) {
        cell.ClearCell();
        Alive = false;
        animator.SetTrigger("Die");

        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
}
